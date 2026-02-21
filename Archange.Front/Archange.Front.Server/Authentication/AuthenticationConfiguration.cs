namespace Archange.Front.Server.Authentication;

/// <summary>
/// OAUth Provider configuration
/// </summary>
public class AuthenticationConfiguration
{
    /// <summary>
    /// Server CallbackPath
    /// </summary>
    /// <example> The server url Callback, usually (signin-{provider}) like <see href="signin-google" /></example>
    public required string CallbackPath { get; set; }
    /// <summary>
    /// Authorization endpoint
    /// </summary>
    public required string AuthorizationEndpoint { get; set; }

    /// <summary>
    /// Token Endpoint
    /// </summary>
    public required string TokenEndpoint { get; set; }

    /// <summary>
    /// User information endpoint
    /// </summary>
    public required string UserInformationEndpoint { get; set; }

    /// <summary>
    /// Claim Issuer
    /// </summary>
    public required string ClaimsIssuer { get; set; }

    /// <summary>
    /// Client Id
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    /// Client Secret
    /// </summary>
    public string? ClientSecret { get; set; }
}
