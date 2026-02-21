using Abstraction.Database;
using System.ComponentModel.DataAnnotations;

namespace Model.Contact;

/// <summary>
/// Phone Number Model
/// </summary>
public class PhoneNumberModel : Entity<Guid>
{
    /// <summary>
    /// Country (references <see cref="CountryModel"/> Name)
    /// </summary>
    [MaxLength(10)]
    public required string Country { get; set; }

    /// <summary>
    /// Phone number
    /// </summary>
    [MaxLength(50)]
    public required string Number { get; set; }
}
