namespace ArchAnge.ServiceDefaults.Sql;

public class SqlQueryBuilderOption
{
    public string? OpenQuote { get; set; }
    public string? CloseQuote { get; set; }
    public required string ParameterPrefix { get; set; }
    public string? TablePrefix { get; set; }
    public required string JoinSeparator { get; set; }
    public required string SetSeparator { get; set; }
}
