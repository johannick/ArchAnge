using Abstraction.Database;
using System.ComponentModel.DataAnnotations;

namespace Model.Hub;

/// <summary>
/// Message model
/// </summary>
public class MessageModel
{
    /// <summary>
    /// Message type, see <see cref="Abstraction.Database.MediaType"/>
    /// </summary>
    public required MediaType MediaType { get; set; }

    /// <summary>
    /// From user identifier
    /// </summary>
    public required Guid FromIdProfile { get; set; }

    /// <summary>
    /// Room Identifier
    /// </summary>
    public required Guid ToIdRoom { get; set; }

    /// <summary>
    /// Creation time
    /// </summary>
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// Message content
    /// </summary>
    [MaxLength(102400000)]
    public required byte[] Content { get; set; }
}
