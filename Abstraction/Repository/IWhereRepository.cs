using Abstraction.Database;
using System.Security.Principal;

namespace Abstraction.Repository;

/// <summary>
/// Where repository
/// </summary>
/// <typeparam name="TEntity"> Entity type </typeparam>
public interface IWhereRepository<out TEntity>
{
    /// <summary>
    /// Where operation
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="specifications"></param>
    /// <returns></returns>
    IAsyncEnumerable<TEntity> Where(IEntity entity, params string[] specifications);

    /// <summary>
    /// Where operation with context
    /// </summary>
    /// <returns></returns>
    IAsyncEnumerable<TEntity> Where();
}
