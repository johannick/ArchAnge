using Microsoft.AspNetCore.Authorization;
using Abstraction.Database;
using System.Data;

namespace ArchAnge.ServiceDefaults.Authorize;

/// <summary>
/// Api Controller and method Authorization
/// </summary>
/// <param name="roles"></param>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class RoleAuthorizeAttribute : AuthorizeAttribute
{
    public IEnumerable<Role> Allowed { get; }

    public RoleAuthorizeAttribute(params Role[] roles)
    {
        Allowed = roles.Distinct();
        Roles = string.Join(',', Allowed.Select(role => Enum.GetName(role)));
    }
}

