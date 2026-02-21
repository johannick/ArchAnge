using Abstraction;
using Abstraction.Database;
using Abstraction.Transform;
using Model.Hub;

namespace Model.Media;

/// <summary>
/// Message transformer
/// </summary>
public class MessageTransformer : IParser<MessageModel>
{
    private Dictionary<MediaType, Func<MessageModel, IMedia?>> Transformers { get; } = new Dictionary<MediaType, Func<MessageModel, IMedia?>>
    {
        { MediaType.Text, ParseText },
        { MediaType.Image, ParseImage }
    };

    /// <summary>
    /// Media
    /// </summary>
    public IMedia? Media { get; set; }

    /// <summary>
    /// Can parse message
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public bool CanParse(MessageModel message)
    {
        return Transformers.ContainsKey(message.MediaType);
    }

    /// <summary>
    /// Parse
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public bool Parse(MessageModel message)
    {
        Media = Transformers[message.MediaType](message);
        return Media != null;
    }

    public static IMedia? ParseText(MessageModel message)
    {
        return IMedia.FromByteArray<TextMedia>(message.Content);
    }

    public static IMedia? ParseImage(MessageModel message) 
    {
        return IMedia.FromByteArray<ImageMedia>(message.Content);
    }
}
