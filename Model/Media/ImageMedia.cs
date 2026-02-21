using Abstraction;

public enum ImageExtension
{
    None,
    jpg,
    jpeg,
    png,
    bmp
}

/// <summary>
/// Image Media
/// </summary>
public class ImageMedia : IMedia
{
    /// <summary>
    /// Encoding type
    /// </summary>
    public required  ImageExtension Extension { get; set; }

    /// <summary>
    /// Message
    /// </summary>
    public required byte[] Raw { get; set; }
}
