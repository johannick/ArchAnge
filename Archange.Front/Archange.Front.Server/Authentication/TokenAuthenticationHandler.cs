using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Options;
using Model.User;
using System.Security.Authentication;
using System.Text.Encodings.Web;

namespace ApiService.Authentication;

/// <summary>
/// Token authentication handler
/// </summary>
/// <param name="options"></param>
/// <param name="logger"></param>
/// <param name="encoder"></param>
public class TokenAuthenticationHandler
    (IOptionsMonitor<OAuthOptions> options
    , ILoggerFactory logger
    , UrlEncoder encoder)
    : OAuthHandler<OAuthOptions>
    (options
    , logger
    , encoder)
{
    /// <summary>
    /// OAuthHandler implementation
    /// </summary>
    /// <param name="properties"></param>
    /// <returns></returns>
    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        if (string.IsNullOrEmpty(properties.RedirectUri))
        {
            properties.RedirectUri = OriginalPathBase + OriginalPath + Request.QueryString;
        }

        // OAuth2 10.12 CSRF
        GenerateCorrelationId(properties);

        var authorizationEndpoint = BuildChallengeUrl(properties, BuildRedirectUri(Options.CallbackPath));

        properties.Items[nameof(AccountModel.AuthenticationUrl)] = authorizationEndpoint;
        await Task.CompletedTask;
    }

    /// <summary>
    /// Authentication implementation
    /// </summary>
    /// <returns> <see cref="AuthenticateResult" /> </returns>
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (Context.User.Identity?.IsAuthenticated ?? false)
        {
            var ticket = new AuthenticationTicket(Context.User, Scheme.Name);

            return await Task.FromResult(AuthenticateResult.Success(ticket));
        }
        return await Task.FromResult(AuthenticateResult.Fail(new AuthenticationException()));
    }
}
