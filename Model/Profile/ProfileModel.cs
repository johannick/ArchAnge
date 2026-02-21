using Abstraction.Database;
using Model.User;
using System.ComponentModel.DataAnnotations;

namespace Model.Profile;

/// <summary>
/// Profile model
/// </summary>
public class ProfileModel : UserModel
{
    /// <summary>
    /// Profile name
    /// </summary>
    [MaxLength(50)]
    public required string Name { get; set; }

    /// <summary>
    /// Profile Avatar
    /// </summary>
    public required string Avatar { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    [MaxLength(300)]
    public required string Description { get; set; }

    /// <summary>
    /// Date of birth
    /// </summary>
    public required DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// Lastname
    /// </summary>
    [MaxLength(25)]
    public string? LastName { get; set; }

    /// <summary>
    /// Firstname
    /// </summary>
    [MaxLength(25)]
    public string? FirstName { get; set; }

    /// <summary>
    /// Gender, see <see cref="Abstraction.Database.Gender"/>
    /// </summary>
    public required Gender Gender { get; set; }

    /// <summary>
    /// Profile Pictures
    /// </summary>
    public IList<string> Pictures { get; set; } = [];

    /// <summary>
    /// Profile interests
    /// </summary>
    public ProfileInterestModel Interests { get; set; } = [];

    /// <summary>
    /// Profile Addresses
    /// </summary>
    public ProfileLocationModel Addresses { get; set; } = [];

    /// <summary>
    /// Profile phones
    /// </summary>
    public ProfilePhoneModel Phones { get; set; } = [];
}
