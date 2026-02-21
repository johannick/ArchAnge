namespace Abstraction.Repository;

/// <summary>
/// Insert repository
/// </summary>
/// <typeparam name="TEntity"> Entity type</typeparam>
public interface IInsertRepository<TEntity>
{
    /// <summary>
    /// Insert operation
    /// </summary>
    /// <param name="entity"> entity to insert</param>
    /// <returns> the updated entity </returns>
    Task<TEntity> Insert(TEntity entity);
}
