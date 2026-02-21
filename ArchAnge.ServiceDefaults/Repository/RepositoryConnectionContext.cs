using Dapper;
using Abstraction.Database;
using Abstraction.Repository;

namespace ArchAnge.ServiceDefaults.Repository;

/// <summary>
/// Connection Repository Context Async
/// <typeparamref name="TEntity"/>
/// <paramref name="builder" />
/// <paramref name="singleSpecifications" />
/// <paramref name="uniqueSpecifications" />
/// </summary>
public class RepositoryConnectionContext<TEntity>
    (ConnectionRequestContext requestContext
    , IDatabaseQueryBuilder<TEntity> builder
    , IEnumerable<string> singleSpecifications
    , params IEnumerable<string> uniqueSpecifications)
    : IRepository<TEntity>
    where TEntity : class
{
    public ConnectionRequestContext Context { get; } = requestContext;
    private IDatabaseQueryBuilder<TEntity> Builder { get; } = builder;
    private string[] SingleSpecifications { get; } = [.. singleSpecifications];
    private string[] UniqueSpecifications { get; } = [.. uniqueSpecifications];

    public async Task<bool> Delete(TEntity entity)
    {
        Context.RequestAborted.ThrowIfCancellationRequested();

        return await Context.Connection.ExecuteAsync(Builder.Delete(SingleSpecifications).Query, entity, Context.Transaction).ConfigureAwait(false) == 1;
    }

    public async IAsyncEnumerable<Tuple<bool, TEntity>> Delete(IEnumerable<TEntity> entities)
    {
        using var scoped = Context.BeginTransaction();
        var query = Builder.Delete(SingleSpecifications).Query;

        foreach (var entity in entities)
        {
            Context.RequestAborted.ThrowIfCancellationRequested();
            yield return Tuple.Create(await Context.Connection.ExecuteAsync(query, entity, Context.Transaction).ConfigureAwait(false) == 1, entity);
        }
    }

    public async IAsyncEnumerable<TEntity> GetAll()
    {
        Context.RequestAborted.ThrowIfCancellationRequested();
        var result = await Context.Connection.QueryAsync<TEntity>(Builder.Select().Query, Context).ConfigureAwait(false);

        foreach (var item in result)
        {
            Context.RequestAborted.ThrowIfCancellationRequested();
            yield return item;
        }
    }

    public async Task<TEntity> Insert(TEntity entity)
    {
        Context.RequestAborted.ThrowIfCancellationRequested();
        return await Context.Connection.QuerySingleAsync<TEntity>(Builder.Insert().Query, entity).ConfigureAwait(false);
    }

    public async IAsyncEnumerable<Tuple<bool, TEntity>> Insert(IEnumerable<TEntity> entities)
    {
        using var scoped = Context.BeginTransaction();
        var query = Builder.Insert().Query;

        foreach (var entity in entities)
        {
            Context.RequestAborted.ThrowIfCancellationRequested();
            var inserted = await Context.Connection.QuerySingleOrDefaultAsync<TEntity?>(query, entity, Context.Transaction).ConfigureAwait(false);

            yield return Tuple.Create(inserted != null, inserted ?? entity);
        }
    }

    public async Task<TEntity> Single(TEntity entity)
    {
        Context.RequestAborted.ThrowIfCancellationRequested();

        return await Context.Connection.QuerySingleAsync<TEntity>(Builder.Single(SingleSpecifications).Query, entity).ConfigureAwait(false);
    }

    public async Task<TEntity?> SingleOrDefault(TEntity entity)
    {
        Context.RequestAborted.ThrowIfCancellationRequested();

        return await Context.Connection.QuerySingleOrDefaultAsync<TEntity>(Builder.Single(SingleSpecifications).Query, entity).ConfigureAwait(false);
    }

    public async Task<TEntity> Unique(TEntity entity)
    {
        Context.RequestAborted.ThrowIfCancellationRequested();

        if (UniqueSpecifications.Length == 0)
            return await Context.Connection.QuerySingleAsync<TEntity>(Builder.Single(SingleSpecifications).Query, entity).ConfigureAwait(false);
        return await Context.Connection.QuerySingleAsync<TEntity>(Builder.Single(UniqueSpecifications).Query, entity).ConfigureAwait(false);
    }

    public async Task<TEntity?> UniqueOrDefault(TEntity entity)
    {
        Context.RequestAborted.ThrowIfCancellationRequested();

        if (UniqueSpecifications.Length == 0)
            return await Context.Connection.QuerySingleOrDefaultAsync<TEntity>(Builder.Single(SingleSpecifications).Query, entity).ConfigureAwait(false);
        return await Context.Connection.QuerySingleOrDefaultAsync<TEntity>(Builder.Single(UniqueSpecifications).Query, entity).ConfigureAwait(false);
    }

    public async Task<bool> Update(TEntity entity)
    {
        Context.RequestAborted.ThrowIfCancellationRequested();
        return await Context.Connection.ExecuteAsync(Builder.Update(SingleSpecifications).Query, entity).ConfigureAwait(false) == 1;
    }

    public async IAsyncEnumerable<Tuple<bool, TEntity>> Update(IEnumerable<TEntity> entities)
    {
        using var scoped = Context.BeginTransaction();
        var query = Builder.Update(SingleSpecifications).Query;

        foreach (var entity in entities)
        {
            Context.RequestAborted.ThrowIfCancellationRequested();
            yield return Tuple.Create(await Context.Connection.ExecuteAsync(query, entity, Context.Transaction) == 1, entity);
        }
    }

    public async IAsyncEnumerable<TEntity> Where(IEntity entity, params string[] specifications)
    {
        Context.RequestAborted.ThrowIfCancellationRequested();
        var result = await Context.Connection.QueryAsync<TEntity>(Builder.Select().Where(specifications).Query, entity);

        foreach (var item in result)
        {
            Context.RequestAborted.ThrowIfCancellationRequested();
            yield return item;
        }
    }

    public IAsyncEnumerable<TEntity> Where() => Where(new Entity<Guid?> { Id = Context.Id }, nameof(Entity<Guid>.Id));
}