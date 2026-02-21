using Abstraction.Database;

namespace Model.Hub;

/// <summary>
/// Match model
/// </summary>
public class MatchModel
{
    /// <summary>
    /// Satus, see <see cref="MatchStatus"/>
    /// </summary>
    public required MatchStatus Status { get; set; }

    /// <summary>
    /// CreatedAt
    /// </summary>
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// Room identifier, if status is <see cref="MatchStatus.Accepted"/>
    /// </summary>
    public Guid? IdRoom { get; set; }

    /// <summary>
    /// From profile identifier
    /// </summary>
    public Guid FromIdProfile { get; set; }

    /// <summary>
    /// To profile identifier
    /// </summary>
    public Guid ToIdProfile { get; set; }
}
