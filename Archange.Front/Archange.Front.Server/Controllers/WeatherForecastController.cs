using Microsoft.AspNetCore.Mvc;

namespace Archange.Front.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Google", "Gitlab", "Snapchat"
    ];

    [HttpGet]
    public IEnumerable<string> Providers()
    {
        return Summaries;
    }
}
