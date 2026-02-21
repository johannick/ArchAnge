using System.ComponentModel.DataAnnotations;

namespace Model.Interest;

/// <summary>
/// Interest model
/// </summary>
public class InterestModel
{
    /// <summary>
    /// Category name, references <see cref="InterestCategoryModel.Name"/>
    /// </summary>
    [MaxLength(50)]
    public required string Category { get; set; }

    /// <summary>
    /// Interest name
    /// </summary>
    [MaxLength(50)]
    public required string Name { get; set; }
}