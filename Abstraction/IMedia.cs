
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;

namespace Abstraction;

/// <summary>
/// IMedia
/// </summary>
public interface IMedia
{
    public static byte[] ToByteArray<T>(T obj)
    {
        using var memorystream = new MemoryStream();
        
        JsonSerializer.Serialize(memorystream, obj);
        return memorystream.ToArray();        
    }

    public static T? FromByteArray<T>(byte[] data)
    {
        using var memorystream = new MemoryStream(data);
        
        return JsonSerializer.Deserialize<T>(memorystream);        
    }
}
