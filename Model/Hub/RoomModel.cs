using Abstraction.Database;

namespace Model.Hub;

/// <summary>
/// Room moldel
/// </summary>
public class RoomModel : Entity<Guid>
{
    /// <summary>
    /// Creation time
    /// </summary>
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// Room owner
    /// </summary>
    public Guid? IdOwner { get; set; }

    /// <summary>
    /// Room type <see cref="Abstraction.Database.RoomType"/>
    /// </summary>
    public required RoomType RoomType { get; set; }
}
