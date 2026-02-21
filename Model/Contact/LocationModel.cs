using Abstraction.Database;
using System.ComponentModel.DataAnnotations;

namespace Model.Contact;

/// <summary>
/// Location Model
/// </summary>
public class LocationModel : Entity<Guid>
{
    /// <summary>
    /// Street
    /// </summary>
    [MaxLength(200)]
    public required string Street { get; set; }

    /// <summary>
    /// Postal Code
    /// </summary>
    [MaxLength(50)]
    public required string PostalCode { get; set; }

    /// <summary>
    /// City
    /// </summary>
    [MaxLength(50)]
    public required string City { get; set; }

    /// <summary>
    /// Country name, (reference <see cref="CountryModel.Name"/>)
    /// </summary>
    [MaxLength(10)]
    public required string Country { get; set; }
}