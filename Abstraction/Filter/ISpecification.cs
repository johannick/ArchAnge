
namespace Abstraction.Filter;

/// <summary>
/// Specification design pattern
/// </summary>
/// <typeparam name="T"> parameter type </typeparam>
public interface ISpecification<in T>
{
    /// <summary>
    /// Predicate
    /// </summary>
    Predicate<T> Filter { get; }
}
