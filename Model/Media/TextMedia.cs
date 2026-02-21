using Abstraction;

namespace Model.Media;

/// <summary>
/// Encoding type
/// </summary>
public enum EncodingType
{
    /// <summary>
    /// UTF8
    /// </summary>
    Utf8,

    /// <summary>
    /// UTF32
    /// </summary>
    Utf32,

    /// <summary>
    /// ASCII
    /// </summary>
    Ascii
}

/// <summary>
/// Text media
/// </summary>
public class TextMedia : IMedia
{
    /// <summary>
    /// Encoding type
    /// </summary>
    public required EncodingType EncodingType { get; set; }

    /// <summary>
    /// Message
    /// </summary>
    public required string Message { get; set; }
}
