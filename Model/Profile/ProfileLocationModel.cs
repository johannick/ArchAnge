using Model.Contact;

namespace Model.Profile;

/// <summary>
/// Profile location model
/// </summary>
public class ProfileLocationModel : List<LocationModel>
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public ProfileLocationModel() { }

    /// <summary>
    /// Constructor from values
    /// </summary>
    /// <param name="entities"> location models </param>
    public ProfileLocationModel(IEnumerable<LocationModel> entities) : base(entities) { }
}
