namespace Abstraction.Repository;

/// <summary>
/// Bulk delete repository
/// </summary>
/// <typeparam name="TEntity"> Entity type </typeparam>
public interface IDeleteRepository<in TEntity>
{
    /// <summary>
    /// Delete operation
    /// </summary>
    /// <param name="entity"> entity to delete </param>
    /// <returns> true if delete was successfull </returns>
    Task<bool> Delete(TEntity entity);
}
