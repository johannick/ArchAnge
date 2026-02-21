using Abstraction.Database;
using System.ComponentModel.DataAnnotations;

namespace Model.Hub;

public class RoomParticipantModel : IEntity
{
    [Key]
    public required Guid IdRoom { get; set; }

    [Key]
    public required Guid IdProfile { get; set; }
}

