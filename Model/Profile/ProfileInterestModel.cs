using Model.Interest;

namespace Model.Profile;

/// <summary>
/// Profile interest model
/// </summary>
public class ProfileInterestModel : List<InterestModel>
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public ProfileInterestModel() { }

    /// <summary>
    /// Constructor with values
    /// </summary>
    /// <param name="collection"></param>
    public ProfileInterestModel(IEnumerable<InterestModel> collection) : base(collection)
    {
    }
}
