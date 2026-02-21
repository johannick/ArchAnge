namespace Archange.Front.Server.Smtp;

/// <summary>
/// Smto options
/// </summary>
public class SmtpOptions
{
    /// <summary>
    /// Username
    /// </summary>
    public required string User { get; set; }

    /// <summary>
    /// Dashboaard url
    /// </summary>
    public required string Dashboard { get; set; }

    /// <summary>
    /// Sender
    /// </summary>
    public required string Sender { get; set; }

    /// <summary>
    /// Host
    /// </summary>
    public required string Host { get; set; }

    /// <summary>
    /// Port
    /// </summary>
    public int Port { get; set; }
}
