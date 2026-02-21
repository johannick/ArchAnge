namespace Abstraction.Repository;

/// <summary>
/// Bulk insert repository
/// </summary>
/// <typeparam name="TEntity"> Entity type </typeparam>
public interface IBulkInsertRepository<TEntity>
{
    /// <summary>
    /// Insert operation
    /// </summary>
    /// <param name="entities"> entities to insert </param>
    /// <returns> true paired if entity was inserted </returns>
    IAsyncEnumerable<Tuple<bool, TEntity>> Insert(IEnumerable<TEntity> entities);
}
