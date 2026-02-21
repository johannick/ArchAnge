using Abstraction.Database;
using Abstraction.Filter;
using Abstraction.Repository;
using ArchAnge.ServiceDefaults.Sql;
using System.Data;

namespace ArchAnge.ServiceDefaults.Repository;

public class MemoryContextRepository<TEntity, TKey> : Dictionary<TKey, TEntity>, IRepository<TEntity>
    where TEntity : IEntity<TKey>
    where TKey : notnull
{
    private Type EntityType { get; }
    private DatabaseAttributesProvider AttributeProvider { get; }
    public RequestContext Context { get; set; }

    public MemoryContextRepository(RequestContext context)
    {
        EntityType = typeof(TEntity);
        AttributeProvider = new(EntityType);
        Context = context;
    }

    public async Task<bool> Delete(TEntity entity)
    {
        Context.RequestAborted.ThrowIfCancellationRequested();
        return await Task.FromResult(Remove(entity.Id));
    }

    public async IAsyncEnumerable<Tuple<bool, TEntity>> Delete(IEnumerable<TEntity> entities)
    {
        foreach (var item in entities)
        {
            Context.RequestAborted.ThrowIfCancellationRequested();
            yield return await Task.FromResult(Tuple.Create(Remove(item.Id), item));
        }
    }

    public async IAsyncEnumerable<TEntity> GetAll()
    {
        foreach (var item in Values)
        {
            Context.RequestAborted.ThrowIfCancellationRequested();
            yield return await Task.FromResult(item);
        }
    }

    public async Task<TEntity> Insert(TEntity entity)
    {
        Context.RequestAborted.ThrowIfCancellationRequested();
        Add(entity.Id, entity);
        return await Task.FromResult(entity);
    }

    public async IAsyncEnumerable<Tuple<bool, TEntity>> Insert(IEnumerable<TEntity> entities)
    {
        foreach (var item in entities)
        {
            Context.RequestAborted.ThrowIfCancellationRequested();
            yield return await Task.FromResult(Tuple.Create(TryAdd(item.Id, item), item));
        }
    }

    public async Task<TEntity> Single(TEntity entity)
    {
        Context.RequestAborted.ThrowIfCancellationRequested();

        if (TryGetValue(entity.Id, out var entityValue))
            return await Task.FromResult(entityValue);
        return await Task.FromResult(entity);
    }

    public async Task<TEntity?> SingleOrDefault(TEntity entity)
    {
        Context.RequestAborted.ThrowIfCancellationRequested();

        TryGetValue(entity.Id, out var entityValue);
        return await Task.FromResult(entityValue);
    }

    public async Task<TEntity> Unique(TEntity entity)
    {
        var uniqueProperties = AttributeProvider.Unique.Select(unique => typeof(TEntity).GetProperty(unique)).ToList();
        var specification = new Specification<TEntity>(entityWhere => uniqueProperties.All(p => p?.GetValue(entityWhere)?.Equals(p.GetValue(entity)) ?? false));

        Context.RequestAborted.ThrowIfCancellationRequested();
        return await Task.FromResult(Values.Single(source => specification.Filter(source)));
    }

    public async Task<TEntity?> UniqueOrDefault(TEntity entity)
    {
        var uniqueProperties = AttributeProvider.Unique.Select(unique => typeof(TEntity).GetProperty(unique)).ToList();
        var specification = new Specification<TEntity>(entityWhere => uniqueProperties.All(p => p?.GetValue(entityWhere)?.Equals(p.GetValue(entity)) ?? false));

        Context.RequestAborted.ThrowIfCancellationRequested();
        return await Task.FromResult(Values.SingleOrDefault(source => specification.Filter(source)));
    }

    public async Task<bool> Update(TEntity entity)
    {
        Context.RequestAborted.ThrowIfCancellationRequested();

        this[entity.Id] = entity;
        return await Task.FromResult(true);
    }

    public async IAsyncEnumerable<Tuple<bool, TEntity>> Update(IEnumerable<TEntity> entities)
    {
        foreach (var item in entities)
        {
            Context.RequestAborted.ThrowIfCancellationRequested();
            yield return Tuple.Create(await Update(item), item);
        }
    }

    public async IAsyncEnumerable<TEntity> Where(IEntity entity, params string[] specifications)
    {
        var properties = specifications.Select(specification => typeof(TEntity).GetProperty(specification.ToString())).ToList();
        var specification = new Specification<TEntity>(entityWhere => properties.All(p => p?.GetValue(entityWhere)?.Equals(p.GetValue(entity)) ?? false));
        var values = Values.Where(value => specification.Filter(value));

        foreach (var value in values)
        {
            Context.RequestAborted.ThrowIfCancellationRequested();
            yield return await Task.FromResult(value);
        }
    }

    public IAsyncEnumerable<TEntity> Where() => Where(new Entity<Guid?> { Id = Context.Id }, nameof(Entity<Guid>.Id));

}
