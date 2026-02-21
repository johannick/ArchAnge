namespace Abstraction.Database.Annotations;

/// <summary>
/// Unique attribute means you can call Single() method on this attribute
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class UniqueAttribute : Attribute
{
}
