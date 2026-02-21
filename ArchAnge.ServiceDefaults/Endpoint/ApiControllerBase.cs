using Microsoft.AspNetCore.Mvc;
using Abstraction;
using Abstraction.Database;
using ArchAnge.ServiceDefaults.Authorize;
using System.Data;

namespace ArchAnge.ServiceDefaults.Endpoint;

/// <summary>
/// Api Controller Base
/// </summary>
[ApiController]
[Route("api/{version:releaseVersion}/[controller]/[action]")]
[ApiExplorerSettings(GroupName = Release.V1)]
[RoleAuthorize(Role.Normal)]
public abstract class ApiControllerBase : ControllerBase
{
}
