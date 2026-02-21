
using ArchAnge.ServiceDefaults.Version;
using System.Diagnostics.CodeAnalysis;

namespace ArchAnge.Front.Server.Swagger; 

/// <summary>
/// Attribute for documentation examples
/// </summary>
[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public sealed class SwaggerExampleAttribute : Attribute
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value"></param>
    [SetsRequiredMembers]
    public SwaggerExampleAttribute(string value) => (Name, Value) = (value, value);

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    [SetsRequiredMembers]
    public SwaggerExampleAttribute(string name, string value) => (Name, Value) = (name, value);

    /// <summary>
    /// Property Name
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Value
    /// </summary>
    public required string Value { get; init; }

    /// <summary>
    /// Version examples
    /// </summary>
    public static readonly IEnumerable<SwaggerExampleAttribute> VersionAttributes = ReleaseProvider.All.Select(static version => new SwaggerExampleAttribute(nameof(version), version));
}
