using Abstraction.Database;
using Model.Contact;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Model.Profile;
public class ProfileAddressModel : IEntity
{
    [SetsRequiredMembers]
    public ProfileAddressModel(IEntity<Guid> profile, string name, LocationModel location) => (IdProfile, Name, IdLocation) = (profile.Id, name, location.Id);

    [Key]
    public required Guid IdProfile { get; set; }

    [Key]
    public required Guid IdLocation { get; set; }

    public required string Name { get; set; }
}

