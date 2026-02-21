namespace Abstraction.Repository;

/// <summary>
/// Bulk delete repository
/// </summary>
/// <typeparam name="TEntity"> Entity type </typeparam>
public interface IBulkDeleteRepository<TEntity>
{
    /// <summary>
    /// Delete operation
    /// </summary>
    /// <param name="entities"> entities to delete </param>
    /// <returns> true paired if entity was deleted </returns>
    IAsyncEnumerable<Tuple<bool, TEntity>> Delete(IEnumerable<TEntity> entities);
}
