using Model.Validations.Results;
using System.ComponentModel.DataAnnotations;

namespace Model.Validation;

/// <summary>
/// Guid validation attribute
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class GuidAttribute : ValidationAttribute
{
    /// <summary>
    /// IsValid Guid
    /// </summary>
    /// <param name="value"></param>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is Guid guid)
        {
            if (guid == Guid.Empty)
            {
                return new EmptyGuidResult();
            }
            return ValidationResult.Success;
        }
        return new InvalidGuidResult();
    }
}
