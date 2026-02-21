using Abstraction.Transform;
using System.Security.Claims;

namespace Abstraction.Database;

/// <summary>
/// Repository Context
/// </summary>
public class RequestContext
{
    /// <summary>
    /// Guid transformer
    /// </summary>
    public static readonly GuidTransformer GuidTransformer = new();

    /// <summary>
    /// Date transformer
    /// </summary>
    public static readonly DateOnlyTransformer DateOnlyTransformer = new();

    /// <summary>
    /// Request cancelled
    /// <see cref="CancellationToken"/> <seealso cref="CancellationTokenSource"/>
    /// </summary>
    public CancellationToken RequestAborted { get; set; }

    /// <summary>
    /// User identity
    /// </summary>
    public readonly ClaimsIdentity Identity = new();

    /// <summary>
    /// Authenticate user
    /// </summary>
    /// <param name="claimType"></param>
    /// <param name="transform"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public void AddClaim<TValue>(ProfileClaims claimType, TValue value, ITransform<TValue> transform)
    {
        var serial = transform.Serialize(value);
        var type = claimType.ToString();
        var claim = new Claim(type, serial);

        if (Identity.HasClaim(type, serial))
        {
            Identity.RemoveClaim(claim);
        }
        Identity.AddClaim(claim);
    }

    /// <summary>
    /// Add role
    /// </summary>
    /// <param name="role"></param>
    /// <returns> true if role is added </returns>
    public bool AddRole(Role role)
    {
        if (Identity.HasClaim(ClaimTypes.Role, role.ToString()))
        {
            return true;
        }
        Identity.AddClaim(new(ClaimTypes.Role, role.ToString()));
        return true;
    }

    /// <summary>
    /// Is in role
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    public bool IsInRole(Role role) => Identity.HasClaim(ClaimTypes.Role, role.ToString());

    /// <summary>
    /// Get claim value
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public string? GetValue(string type) => Identity.FindFirst(type)?.Value;

    /// <summary>
    /// Get email
    /// </summary>
    /// <returns></returns>
    public string? Email => GetValue(nameof(ProfileClaims.Email));

    /// <summary>
    /// Get user Id
    /// </summary>
    /// <returns></returns>
    public Guid? Id
    {
        get
        {
            var guidSerial = GetValue(nameof(ProfileClaims.UserId)) ?? string.Empty;

            if (GuidTransformer.TryParse(guidSerial, out Guid identifier))
                return identifier;
            return null;
        }
    }

    /// <summary>
    /// User age
    /// </summary>
    /// <returns></returns>
    public int? Age
    {
        get
        {
            var dateSerial = GetValue(nameof(ProfileClaims.DateOfBirth)) ?? string.Empty;

            if (DateOnlyTransformer.TryParse(dateSerial, out DateOnly dateOfBirth))
                return dateOfBirth.Age();
            return null;
        }
    }
}
