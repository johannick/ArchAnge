namespace ArchAnge.ServiceDefaults.Sql;

public class ConnectionStringOption
{
    /// <summary>
    /// Hostname or ip adress
    /// </summary>
    public required string Host { get; set; }

    /// <summary>
    /// Port
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Database name
    /// </summary>
    public required string Database { get; set; }

    /// <summary>
    /// Database username
    /// </summary>
    public required string Username { get; set; }

    /// <summary>
    /// Database Password
    /// </summary>
    public required string Password { get; set; }

    /// <summary>
    /// The time to wait while trying to read a response for a cancellation request for a timed out or cancelled query, 
    /// before terminating the attempt and generating an error. 
    /// 
    /// <list type="bullet">
    /// <item> Zero for infinity </item>
    /// <item> -1 to skip the wait </item>
    /// <item> Defaults to 2000 milliseconds </item>
    /// </list>
    /// </summary>
    public TimeSpan CancellationTimeout { get; set; } = TimeSpan.FromMilliseconds(2000);

    /// <summary>
    /// The time to wait while trying to establish a connection before terminating the attempt and generating an error.
    /// Defaults to 15 seconds.
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(15);
}
