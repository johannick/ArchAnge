using System.Globalization;

namespace Abstraction.Transform;

/// <summary>
/// Serialize date only
/// <paramref name="provider"> format provider </paramref>
/// </summary>
public class DateOnlyTransformer(DateTimeStyles style = DateTimeStyles.AssumeUniversal, IFormatProvider? provider = default) : ITransform<DateOnly>
{
    /// <summary>
    /// Can parse
    /// </summary>
    /// <param name="serial"></param>
    /// <returns></returns>
    public bool CanParse(string serial) => !string.IsNullOrWhiteSpace(serial);

    /// <summary>
    /// Parse
    /// </summary>
    /// <param name="serial"></param>
    /// <returns></returns>
    public DateOnly Parse(string serial) => DateOnly.Parse(serial, provider, style);

    /// <summary>
    /// Serialize
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public string Serialize(DateOnly entity) => entity.ToString(provider);

    /// <summary>
    /// Try parse
    /// </summary>
    /// <param name="entity"> entity </param>
    /// <param name="serial"> serial </param>
    /// <returns></returns>
    public bool TryParse(string serial, out DateOnly entity) => DateOnly.TryParse(serial, provider, style, out entity);
}
