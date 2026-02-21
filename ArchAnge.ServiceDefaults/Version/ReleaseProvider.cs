using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Abstraction;

namespace ArchAnge.ServiceDefaults.Version;

/// <summary>
/// Released versions provider
/// </summary>
public static class ReleaseProvider
{
    static ReleaseProvider()
    {
        All = [];
        ApiVersionDescriptions = [];

        static ApiVersionDescription serialToDescrition(string name, System.Version version)
        {
            All?.Add(name);
            return new ApiVersionDescription(new ApiVersion(DateOnly.FromDateTime(DateTime.Today), version.Major, version.Minor), name, true);
        }

        foreach (var item in Release.Releases)
        {
            ApiVersionDescriptions.Add(serialToDescrition(item.Key, item.Value));
        }
        Default = ApiVersionDescriptions[0];
    }

    public static List<string> All { get; }
    public static ApiVersionDescription Default { get; }
    public static IList<ApiVersionDescription> ApiVersionDescriptions { get; }
}

