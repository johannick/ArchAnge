using System.ComponentModel.DataAnnotations;

namespace Model.Interest;

/// <summary>
/// Interest catefory model
/// </summary>
public class InterestCategoryModel
{
    /// <summary>
    /// Category name
    /// </summary>
    [MaxLength(50)]
    public required string Name { get; set; }
}