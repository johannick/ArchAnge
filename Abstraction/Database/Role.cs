namespace Abstraction.Database;

/// <summary>
/// User Role
/// </summary>
public enum Role
{
    /// <summary>
    /// None if desactivated but accesed by some developers which are not Admin
    /// </summary>
    None,

    /// <summary>
    /// Basic operations
    /// </summary>
    Normal,

    /// <summary>
    /// Reserved operations
    /// </summary>
    Premium,

    /// <summary>
    /// GOD Mode
    /// </summary>
    Administrator
}
