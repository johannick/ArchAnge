using System.ComponentModel.DataAnnotations;

namespace Model.Contact;

/// <summary>
/// Country model
/// </summary>
public class CountryModel
{
    /// <summary>
    /// Country Trigram
    /// </summary>
    [MaxLength(10)]
    public required string Name { get; set; }
}
