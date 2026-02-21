
namespace ArchAnge.ServiceDefaults.Authorize;

/// <summary>
/// Random Serial Generator
/// </summary>
public static class Serial
{
    private const string AllowedCharacters = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRTUVWXYZ-_";

    /// <summary>
    /// Generate a random serial of allowed chars from min length to max length
    /// </summary>
    /// <param name="min">Minimum length</param>
    /// <param name="max">Maximum length </param>
    /// <param name="alphabet"> Allowed chars </param>
    /// <returns> Generated string </returns>
    public static string GenerateShortCode(int min, int max = 0, string alphabet = AllowedCharacters)
    {
        min = Math.Max(1, min);
        max = Math.Max(min, max);

        var random = new Random();
        var number = random.Next(min, max);

        return new string([.. Enumerable.Range(0, number).Select(_ => alphabet[random.Next(0, alphabet.Length - 1)])]);
    }
}
