namespace Abstraction.Repository;

/// <summary>
/// Single repository
/// </summary>
/// <typeparam name="TEntity"> Entity type</typeparam>
public interface ISingleRepository<TEntity>
{
    /// <summary>
    /// Single operation
    /// </summary>
    /// <param name="entity"> Entity to look for </param>
    /// <returns> Actual entity </returns>
    Task<TEntity> Single(TEntity entity);
}
