namespace Abstraction.Transform;

/// <summary>
/// Interface for media
/// <typeparamref name="TMessage"/>
/// </summary>
public interface IParser<in TMessage>
{
    /// <summary>
    /// Can Parse message content
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    bool CanParse(TMessage message);

    /// <summary>
    /// Parse message
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    bool Parse(TMessage message);
}

/// <summary>
/// Parsing interface
/// </summary>
/// <typeparam name="TSerial"> Serial type </typeparam>
/// <typeparam name="TEntity"> Entity type </typeparam>
public interface IParser<in TSerial, TEntity>
{
    /// <summary>
    /// Can Parse message content
    /// </summary>
    /// <param name="serial"></param>
    /// <returns></returns>
    bool CanParse(TSerial serial);

    /// <summary>
    /// Parse serial
    /// </summary>
    /// <param name="serial"></param>
    /// <returns></returns>
    TEntity Parse(TSerial serial);

    /// <summary>
    /// Parse
    /// </summary>
    /// <param name="serial"> Serial entity </param>
    /// <param name="entity"> entity from serial </param>
    bool TryParse(TSerial serial, out TEntity entity);
}
