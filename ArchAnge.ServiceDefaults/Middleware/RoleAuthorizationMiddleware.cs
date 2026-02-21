using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Abstraction.Database;
using ArchAnge.ServiceDefaults.Authorize;
using Microsoft.Extensions.DependencyInjection;

namespace ArchAnge.ServiceDefaults.Middleware;

/// <summary>
/// Authorization Middleware
/// </summary>
/// <param name="requestContext"></param>
/// <param name="logger"></param>
public class RoleAuthorizationMiddleware(IServiceProvider provider, ILogger<RoleAuthorizationMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var executingEnpoint = context.GetEndpoint();
        var allowAttribute = executingEnpoint?.Metadata.OfType<IAllowAnonymous>().FirstOrDefault();
        var rolesAttribute = executingEnpoint?.Metadata.OfType<RoleAuthorizeAttribute>().FirstOrDefault();
        var connection = provider.GetService<ConnectionRequestContext>();
        var url = context.Request.Path.ToString();

        if (allowAttribute != null || HasRights(connection, rolesAttribute) || !url.Contains("api"))
        {
            await next(context);
        }
        else
        {
            logger.LogWarning("Access not Allowed : {Url}", url);
        }
    }

    private static bool HasRights(RequestContext? context, RoleAuthorizeAttribute? rolesAttribute)
    {
        return rolesAttribute?.Allowed.Any(role => context?.IsInRole(role) ?? false) ?? false;
    }
}
