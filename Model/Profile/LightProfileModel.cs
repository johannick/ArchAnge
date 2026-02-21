using Abstraction.Database;
using Model.Validation;
using System.ComponentModel.DataAnnotations;

namespace Model.Profile;

/// <summary>
/// Light profile model
/// </summary>
public class LightProfileModel : Entity<Guid>
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
}
