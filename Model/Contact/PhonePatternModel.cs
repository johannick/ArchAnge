using Abstraction.Database;
using System.ComponentModel.DataAnnotations;

namespace Model.Contact;

/// <summary>
/// Phone pattern model
/// </summary>
public class PhonePatternModel : Entity<Guid>
{
    /// <summary>
    /// Flag, (references <see cref="CountryModel.Name"/>)
    /// </summary>
    [MaxLength(10)]
    public required string Flag { get; set; }

    /// <summary>
    /// National prefixe
    /// </summary>
    [MaxLength(10)]
    public required string Prefixe { get; set; }
}
