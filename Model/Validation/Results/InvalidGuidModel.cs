using System.ComponentModel.DataAnnotations;

namespace Model.Validations.Results;

/// <summary>
/// Invalid guid validation result
/// </summary>
public class InvalidGuidResult() : ValidationResult("Invalid Guid")
{
}
