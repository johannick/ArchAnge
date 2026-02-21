using Model.Validation;

namespace Model.Profile;

/// <summary>
/// Profile delete pictures model
/// </summary>
public class ProfileDeletePictureModel
{
    /// <summary>
    /// Pictures tro delete
    /// </summary>
    public required IEnumerable<Tuple<int, string>> Pictures { get; set; }
}