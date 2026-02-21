
namespace Abstraction.Database
{
    /// <summary>
    /// Database query builder, powerfull toll for Dapper queries
    /// Or queries itself
    /// </summary>
    /// <typeparam name="TEntity"> Class to load from db </typeparam>
    public interface IDatabaseQueryBuilder<TEntity>
    {
        /// <summary>
        /// Crud operations
        /// </summary>
        /// <returns>  Chainable operation </returns>
        IDatabaseQueryBuilder<TEntity> Select();

        /// <summary>
        /// Crud operations
        /// </summary>
        /// <returns>  Chainable operation </returns>
        IDatabaseQueryBuilder<TEntity> Insert();

        /// <summary>
        /// Crud operations
        /// </summary>
        /// <returns>  Chainable operation </returns>
        IDatabaseQueryBuilder<TEntity> Where(params string[] specifications);

        /// <summary>
        /// Crud operations
        /// </summary>
        /// <returns>  Chainable operation </returns>
        IDatabaseQueryBuilder<TEntity> Single(params string[] specifications);

        /// <summary>
        /// Crud operations
        /// </summary>
        /// <returns>  Chainable operation </returns>
        IDatabaseQueryBuilder<TEntity> Update(params string[] specifications);

        /// <summary>
        /// Crud operations
        /// </summary>
        /// <returns>  Chainable operation </returns>
        IDatabaseQueryBuilder<TEntity> Delete(params string[] specifications);

        /// <summary>
        /// Final Query
        /// </summary>
        string Query { get; }
    }
}
