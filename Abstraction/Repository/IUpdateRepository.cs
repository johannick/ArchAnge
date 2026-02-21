namespace Abstraction.Repository;

/// <summary>
/// Update repository
/// </summary>
/// <typeparam name="TEntity"> Entity type </typeparam>
public interface IUpdateRepository<in TEntity>
{
    /// <summary>
    /// Update operation
    /// </summary>
    /// <param name="entity"> Entity to update </param>
    /// <returns> true if successfull</returns>
    Task<bool> Update(TEntity entity);
}
