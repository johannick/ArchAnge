using Abstraction.Database;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Model.User;

/// <summary>
/// User model
/// </summary>
public class UserModel : UserForgotPasswordModel, IEntity<Guid>
{
    /// <summary>
    /// Password
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// User identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Forgot token, used to reset password
    /// </summary>
    public Guid? ForgotToken { get; set; }

    /// <summary>
    /// Forgot date, used to reset password
    /// </summary>
    public DateTime? ForgotDate { get; set; }
}

