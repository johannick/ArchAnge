namespace Abstraction.Transform;

/// <summary>
/// Serializable packet interface
/// </summary>
/// <typeparam name="TEntity"> Entity type </typeparam>
/// <typeparam name="TSerial"> Serial type </typeparam>
public interface ITransform<TEntity, TSerial>
    : IParser<TSerial, TEntity>
    , ISerial<TEntity, TSerial>
{
}

/// <summary>
/// Serializable to string interface
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface ITransform<TEntity> : ITransform<TEntity, string>
{
}