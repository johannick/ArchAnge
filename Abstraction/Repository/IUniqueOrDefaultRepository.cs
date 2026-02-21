namespace Abstraction.Repository;

/// <summary>
/// Unique or default repository
/// </summary>
/// <typeparam name="TEntity"> Entity type </typeparam>
public interface IUniqueOrDefaultRepository<TEntity>
{
    /// <summary>
    /// Unique or default operation
    /// </summary>
    /// <param name="entity"></param>
    /// <returns> entity if founded </returns>
    Task<TEntity?> UniqueOrDefault(TEntity entity);
}
