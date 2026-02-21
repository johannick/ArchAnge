namespace Abstraction.Filter;

/// <summary>
/// Specification
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="filter"></param>
public class Specification<T>(Predicate<T> filter) : ISpecification<T>
{
    /// <summary>
    /// Filter
    /// </summary>
    public Predicate<T> Filter { get; } = filter;
}
