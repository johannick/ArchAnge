using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Model.Validation;

/// <summary>
/// Validation
/// </summary>
public static class Validation

{
    /// <summary>
    /// Validate
    /// </summary>
    public static bool Validate<TModel>(TModel model) where TModel : class => model.GetType().GetCustomAttributes<ValidationAttribute>().All(validator => validator.IsValid(model));
}