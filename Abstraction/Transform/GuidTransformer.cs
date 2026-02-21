namespace Abstraction.Transform;

/// <summary>
/// Guid Transformer
/// </summary>
/// <param name="format"></param>
/// <param name="provider"></param>
public class GuidTransformer(string format = "D", IFormatProvider? provider = default) : ITransform<Guid>
{
    /// <summary>
    /// Can parse serial
    /// </summary>
    /// <param name="serial"></param>
    /// <returns></returns>
    public bool CanParse(string serial) => serial.Length == Guid.Empty.ToString().Length;

    /// <summary>
    /// Parse serial
    /// </summary>
    /// <param name="serial"></param>
    /// <returns></returns>
    public Guid Parse(string serial) => Guid.Parse(serial);

    /// <summary>
    /// Serialize
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public string Serialize(Guid entity) => entity.ToString(format, provider);

    /// <summary>
    /// Try parse
    /// </summary>
    /// <param name="serial"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool TryParse(string serial, out Guid entity) => Guid.TryParse(serial, out entity);
}
