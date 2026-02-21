namespace Abstraction.Filter;

/// <summary>
/// Specifications extensions
/// </summary>
public static class SpecificationExtensions
{
    /// <summary>
    /// ISpecification And
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static ISpecification<T> And<T>(this ISpecification<T> left, ISpecification<T> right)
    {
        return new Specification<T>(_ => left.Filter(_) && right.Filter(_));
    }

    /// <summary>
    /// ISpecification Or
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static ISpecification<T> Or<T>(this ISpecification<T> left, ISpecification<T> right)
    {
        return new Specification<T>(_ => left.Filter(_) || right.Filter(_));
    }

    /// <summary>
    /// ISpecification Not
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="specification"></param>
    /// <returns></returns>
    public static ISpecification<T> Not<T>(this ISpecification<T> specification)
    {
        return new Specification<T>(_ => !specification.Filter(_));
    }
}
