using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.User;
using System.Net.Mime;
using Archange.Front.Server.Authentication;
using ArchAnge.ServiceDefaults.Endpoint;
using ArchAnge.Front.Server.Swagger;

namespace ApiService.Controllers;

/// <summary>
/// Account controller
/// </summary>
/// <param name="authManager"></param>
public class AccountController
    (AuthenticationConfigurationManager authManager)
    : ApiControllerBase
{

    /// <summary>
    /// Get the list of provider names
    /// For Authentication
    /// </summary>
    /// <example> [ "Google", "Snapchat", "Instagram" ] </example>
    /// <permission cref="IAllowAnonymous"></permission>
    /// <remarks> If there is an exception on this call it means It,s probably Dependency injection</remarks>
    /// <returns> <see cref="string"/> </returns>
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    public IEnumerable<string> Providers()
    {
        return authManager.Keys;
    }

    /// <summary>
    /// OAuth login with another authority
    /// </summary>
    /// <param name="provider">provider name, should be a value from the route above </param>
    /// <permission cref="IAllowAnonymous"></permission>
    /// <returns> <see cref="AccountModel"/> </returns>
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(AccountModel), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    public async Task<JsonResult> Login([SwaggerExample("Google")][SwaggerExample("Github")] string provider)
    {
        var segments = Request.Path.ToString().Split("/").ToList();
        var index = segments.IndexOf("api");
        var properties = new AuthenticationProperties
        {
            RedirectUri = $"/api/{segments[index + 1]}/user"
        };

        await HttpContext.ChallengeAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(false);
        await HttpContext.ChallengeAsync(JwtBearerDefaults.AuthenticationScheme).ConfigureAwait(false);
        await HttpContext.ChallengeAsync(provider, properties);

        var url = properties.Items[nameof(AccountModel.AuthenticationUrl)];

        return await Task.FromResult(new JsonResult(new AccountModel { AuthenticationUrl = url ?? string.Empty }));
    }

    /// <summary>
    /// Logout
    /// </summary>
    /// <returns> Nothing if success </returns>
    [HttpDelete]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignOutAsync(JwtBearerDefaults.AuthenticationScheme);
    }
}
