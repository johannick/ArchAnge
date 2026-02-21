using Model.Contact;

namespace Model.Profile;

/// <summary>
/// Profile Phone model
/// </summary>
public class ProfilePhoneModel : List<PhoneNumberModel>
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public ProfilePhoneModel() { }

    /// <summary>
    /// Constructor from values
    /// </summary>
    /// <param name="entities"> entities, see <see cref="PhoneNumberModel"/> </param>
    public ProfilePhoneModel(IEnumerable<PhoneNumberModel> entities) : base(entities) { }
}