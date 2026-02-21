using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ArchAnge.ServiceDefaults.Middleware;

/// <summary>
/// Logging middleware
/// </summary>
/// <param name="logger"></param>
public sealed class LoggerMiddleware(ILogger<LoggerMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var url = context.Request.Path.Value;
        var watcher = Stopwatch.StartNew();
        using var scope = logger.BeginScope("[+]{Url}", url);

        try
        {
            await next(context);
        }
        catch (System.Exception exception)
        {
            logger.LogError(exception, "Unhandled exception");
        }
        finally
        {
            watcher.Stop();
            logger.LogInformation("[-]{Url} Time {ElapsedMilliseconds:N1}", url, watcher.ElapsedMilliseconds);
        }
    }
}
