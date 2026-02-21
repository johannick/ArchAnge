using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Abstraction;
using Abstraction.Database;
using ArchAnge.ServiceDefaults.Authorize;
using System.Data;

namespace ArchAnge.ServiceDefaults.Endpoint;

[ApiExplorerSettings(GroupName = Release.V1, IgnoreApi = true)]
[RoleAuthorize(Role.Normal)]
public abstract class ApiHubBase : Hub
{
}
