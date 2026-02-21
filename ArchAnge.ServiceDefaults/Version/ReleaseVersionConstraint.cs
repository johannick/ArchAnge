using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ArchAnge.ServiceDefaults.Version;

/// <summary>
/// Release version constraint(check if requested version exist)
/// </summary>
public class ReleaseVersionConstraint : IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (!values.TryGetValue(routeKey, out var routeValue))
        {
            return false;
        }

        return ReleaseProvider.All.Contains(Convert.ToString(routeValue, default) ?? string.Empty);
    }
}
