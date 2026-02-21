using Abstraction.Database;
using ArchAnge.ServiceDefaults.Exception;

namespace ArchAnge.ServiceDefaults.Context;

public static class Extensions
{
    public static readonly string ContentRoot = "webroot/images";

    public static string ImageFolder(this RequestContext context)
    {
        var userId = context.Id ?? throw new UserNotConnectedException();
        var folder = Path.Combine(AppContext.BaseDirectory, ContentRoot, userId.ToString().ToUpper());

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
        return folder;
    }
}