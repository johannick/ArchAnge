namespace Abstraction.Database;

/// <summary>
/// Match Status
/// </summary>
public enum MatchStatus
{
    /// <summary>
    /// Pending if you are the first one to match
    /// </summary>
    Pending,

    /// <summary>
    /// Accepted if you both match
    /// </summary>
    Accepted,

    /// <summary>
    /// Rejected if someone has rejected the offer
    /// </summary>
    Rejected
}
