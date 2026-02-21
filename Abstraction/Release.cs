namespace Abstraction;

/// <summary>
/// Release
/// </summary>
public static class Release
{
    /// <summary>
    /// V1 Release version
    /// </summary>
    public const string V1 = "1.0";

    /// <summary>
    /// Available release in swagger
    /// </summary>
    public static IDictionary<string, Version> Releases { get; } = new Dictionary<string, Version>
    {
        { V1, new Version(1, 0) }
    };
}
