namespace Abstraction.Transform;

/// <summary>
/// Parsing interface
/// </summary>
/// <typeparam name="TEntity"> Entity type </typeparam>
/// <typeparam name="TSerial"> Serial type </typeparam>
public interface ISerial<in TEntity, out TSerial>
{
    /// <summary>
    /// Parse
    /// </summary>
    /// <param name="entity"> entity to serialize </param>
    /// <returns> </returns>
    TSerial Serialize(TEntity entity);
}
