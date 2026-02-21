namespace Abstraction.Repository;

/// <summary>
/// Bulk repository
/// <list type="bullet">
///     <item> Delete operation </item>
///     <item> Update operation </item>
///     <item> Insert operation </item>
/// </list>
/// </summary>
/// <typeparam name="TEntity"> Entity type </typeparam>
public interface IBulkRepository<TEntity> :
    IBulkInsertRepository<TEntity>,
    IBulkDeleteRepository<TEntity>,
    IBulkUpdateRepository<TEntity>
{
}

/// <summary>
/// Crud repository
/// <list type="bullet">
///     <item> GetAll operation </item>
///     <item> Delete operation </item>
///     <item> Update operation </item>
///     <item> Single operation </item>
///     <item> Insert operation </item>
///     <item> Unique operation </item>
/// </list>
/// </summary>
/// <typeparam name="TEntity"> Entity type </typeparam>
public interface ICrudRepository<TEntity> :
    IGetAllRepository<TEntity>,
    IUpdateRepository<TEntity>,
    IDeleteRepository<TEntity>,
    ISingleRepository<TEntity>,
    IInsertRepository<TEntity>,
    IUniqueRepository<TEntity>,
    IWhereRepository<TEntity>,
    ISingleOrDefaultRepository<TEntity>,
    IUniqueOrDefaultRepository<TEntity>
{
}

/// <summary>
/// Full repository
/// </summary>
/// <typeparam name="TEntity"> Entity type </typeparam>
public interface IRepository<TEntity> :
    ICrudRepository<TEntity>,
    IBulkRepository<TEntity>
{
}
