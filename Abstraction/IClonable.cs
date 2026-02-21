
namespace Abstraction;

/// <summary>
/// Clone typed pattern
/// </summary>
/// <typeparam name="T"> Entity to clone </typeparam>
public interface ICloneable<out T>
{
    /// <summary>
    /// Clone operation
    /// </summary>
    /// <returns></returns>
    T Clone();
}
