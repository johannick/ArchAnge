using System.Data;

namespace Abstraction.Database;

/// <summary>
/// <see cref="RequestContext"/> implementation
/// </summary>
public class ConnectionRequestContext : RequestContext, IDisposable
{
    /// <summary>
    /// Database Connection
    /// </summary>
    public IDbConnection? DatabaseConnection { get; set; }

    /// <summary>
    /// Connection
    /// </summary>
    /// <exception cref="ConnectionNullException"></exception>
    public IDbConnection Connection { get => DatabaseConnection ?? throw new ConnectionNullException(); set => DatabaseConnection = value; }

    /// <summary>
    /// Database Transaction
    /// </summary>
    public IDbTransaction? Transaction { get; set; }

    /// <summary>
    /// Begin transaction
    /// </summary>
    /// <returns></returns>
    public IDisposable BeginTransaction()
    {
        Transaction = Connection.BeginTransaction();
        return this;
    }

    /// <summary>
    /// Dispose transaction if exist
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Destructor
    /// </summary>
    ~ConnectionRequestContext()
    {
        Dispose(false);
    }

    /// <summary>
    /// Dispose pattern
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
            return;

        if (Transaction != null)
        {
            Transaction.Commit();
            Transaction.Dispose();
            Transaction = null;
        }
    }
}
