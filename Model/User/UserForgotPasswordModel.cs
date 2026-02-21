using System.ComponentModel.DataAnnotations;

namespace Model.User;

/// <summary>
/// Forgot email model
/// </summary>
public class UserForgotPasswordModel
{
    /// <summary>
    /// Email
    /// </summary>
    [EmailAddress]
    public required string Email { get; set; }
}
