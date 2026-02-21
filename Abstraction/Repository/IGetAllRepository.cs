namespace Abstraction.Repository;

/// <summary>
/// Get all repository
/// </summary>
/// <typeparam name="TEntity"> Entity type </typeparam>
public interface IGetAllRepository<out TEntity>
{
    /// <summary>
    /// Get all operation
    /// </summary>
    /// <returns> All entities as <see cref="IEnumerable{TEntity}"/></returns>
    IAsyncEnumerable<TEntity> GetAll();
}
