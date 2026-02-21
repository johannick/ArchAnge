using System.ComponentModel.DataAnnotations;

namespace Model.Validations.Results;

/// <summary>
/// Empty guid validation result
/// </summary>
public class EmptyGuidResult() : ValidationResult("Guid cannot be empty")
{
}
