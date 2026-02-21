using Abstraction.Database.Annotations;
using Abstraction.Database;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Model.Profile;

public class ProfilePictureModel : IEntity
{
    [SetsRequiredMembers]
    public ProfilePictureModel(IEntity<Guid> profile, string picture) => (IdProfile, Name) = (profile.Id, picture);

    [Key]
    public required Guid IdProfile { get; set; }

    [Key]
    [Unique]
    public required string Name { get; set; }
}
