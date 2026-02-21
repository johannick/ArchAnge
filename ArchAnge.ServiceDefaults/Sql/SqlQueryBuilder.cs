using Abstraction.Database;
using System.Text;

namespace ArchAnge.ServiceDefaults.Sql;

public class SqlQueryBuilder<TEntity> : IDatabaseQueryBuilder<TEntity>
{
    public SqlQueryBuilder(SqlQueryBuilderOption options)
    {
        Options = options;
        Provider = new DatabaseAttributesProvider(typeof(TEntity));
        TableName = Options.OpenQuote + Provider.TableName + Options.CloseQuote;
        QueryHeader = string.Empty;
        QueryFooter = string.Empty;
    }

    public SqlQueryBuilderOption Options { get; }
    protected string QueryHeader { get; set; }
    protected string QueryFooter { get; set; }
    protected string TableName { get; }
    public string Query => (QueryHeader + QueryFooter).ToLower();

    public readonly DatabaseAttributesProvider Provider;

    public IDatabaseQueryBuilder<TEntity> Select()
    {
        var queryBuilder = new StringBuilder();

        queryBuilder.Append("SELECT ");

        var propertySelected = Provider.Properties.Select(Provider.Alias).Select(p => TableName + Options.JoinSeparator + p);

        queryBuilder.Append(string.Join(", ", propertySelected));
        queryBuilder.Append(" FROM ");

        if (!string.IsNullOrEmpty(Options.TablePrefix))
        {
            queryBuilder.Append(Options.TablePrefix);
            queryBuilder.Append(Options.JoinSeparator);
        }
        queryBuilder.Append(TableName);
        QueryHeader = queryBuilder.ToString();
        return this;
    }

    public IDatabaseQueryBuilder<TEntity> Insert()
    {
        var queryBuilder = new StringBuilder();

        queryBuilder.Append("INSERT INTO ");

        if (!string.IsNullOrEmpty(Options.TablePrefix))
        {
            queryBuilder.Append(Options.TablePrefix);
            queryBuilder.Append(Options.JoinSeparator);
        }
        queryBuilder.Append(TableName);

        var properties = Provider.Properties.Except(Provider.DatabaseGenerated);

        queryBuilder.Append('(');
        queryBuilder.Append(string.Join(", ", properties.Select(Provider.Alias)));
        queryBuilder.Append(") VALUES (");
        queryBuilder.Append(string.Join(", ", properties.Select(p => Options.ParameterPrefix + p)));
        queryBuilder.Append(')');
        queryBuilder.Append(" RETURNING *");
        QueryHeader = queryBuilder.ToString();
        return this;
    }

    public IDatabaseQueryBuilder<TEntity> Delete(params string[] specifications)
    {
        var queryBuilder = new StringBuilder();

        queryBuilder.Append("DELETE FROM ");

        if (!string.IsNullOrEmpty(Options.TablePrefix))
        {
            queryBuilder.Append(Options.TablePrefix);
            queryBuilder.Append(Options.JoinSeparator);
        }
        queryBuilder.Append(TableName);
        QueryHeader = queryBuilder.ToString();
        return Where(specifications);
    }

    public IDatabaseQueryBuilder<TEntity> Single(params string[] specifications)
    {
        Select();
        return Where(specifications);
    }

    public IDatabaseQueryBuilder<TEntity> Update(params string[] specifications)
    {
        var queryBuilder = new StringBuilder();

        queryBuilder.Append("UPDATE ");

        if (!string.IsNullOrEmpty(Options.TablePrefix))
        {
            queryBuilder.Append(Options.TablePrefix);
            queryBuilder.Append(Options.JoinSeparator);
        }
        queryBuilder.Append(TableName);
        queryBuilder.Append(" SET ");

        var properties = Provider.Properties.Except(Provider.Keys).Except(Provider.DatabaseGenerated);

        if (!properties.Any())
        {
            throw new MissingMemberException();
        }
        queryBuilder.Append(string.Join(Options.SetSeparator, properties.Select(p => Provider.Alias(p) + " = " + Options.ParameterPrefix + p)));
        QueryHeader = queryBuilder.ToString();
        return Where(specifications);
    }

    public IDatabaseQueryBuilder<TEntity> Where(params string[] specifications)
    {
        var queryBuilder = new StringBuilder();

        queryBuilder.Append(" WHERE ");
        queryBuilder.Append(string.Join(" AND ", specifications.Select(p => TableName + Options.JoinSeparator + Provider.Alias(p.ToString()) + " = " + Options.ParameterPrefix + p)));
        QueryFooter = queryBuilder.ToString();
        return this;
    }
}
