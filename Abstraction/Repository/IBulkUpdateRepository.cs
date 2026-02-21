namespace Abstraction.Repository;

/// <summary>
/// Bulk update repository
/// </summary>
/// <typeparam name="TEntity"> Entity type </typeparam>
public interface IBulkUpdateRepository<TEntity>
{
    /// <summary>
    /// Entities
    /// </summary>
    /// <param name="entities"> entities to update </param>
    /// <returns> true paired if entity was updated </returns>
    IAsyncEnumerable<Tuple<bool, TEntity>> Update(IEnumerable<TEntity> entities);
}

