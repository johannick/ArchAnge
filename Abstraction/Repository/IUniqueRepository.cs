namespace Abstraction.Repository;

/// <summary>
/// Unique repository
/// </summary>
/// <typeparam name="TEntity"> Entity type </typeparam>
public interface IUniqueRepository<TEntity>
{
    /// <summary>
    /// Unique operation
    /// </summary>
    /// <param name="entity"> Entity type </param>
    /// <returns> entity </returns>
    Task<TEntity> Unique(TEntity entity);
}