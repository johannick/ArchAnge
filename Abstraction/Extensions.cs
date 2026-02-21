namespace Abstraction;

/// <summary>
/// Extensions
/// </summary>
public static class Extensions
{
    /// <summary>
    /// For each
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="values"></param>
    /// <param name="function"></param>
    /// <returns></returns>
    public static void ForEach<T>(this IEnumerable<T> values, Action<T> function)
    {
        foreach (var value in values)
        {
            function(value);
        }
    }

    /// <summary>
    /// To list async
    /// </summary>
    /// <typeparam name="T"> item type </typeparam>
    /// <param name="items"> list </param>
    /// <param name="cancellationToken"> <see cref="CancellationToken"/> <seealso cref="CancellationTokenSource"/> </param>
    /// <returns></returns>
    public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> items, CancellationToken cancellationToken = default)
    {
        var results = new List<T>();

        await foreach (var item in items.WithCancellation(cancellationToken).ConfigureAwait(false))
            results.Add(item);
        return results;
    }

    /// <summary>
    /// Current age from dateonly
    /// </summary>
    /// <param name="dateOfBirth"></param>
    /// <returns></returns>
    public static int Age(this DateOnly dateOfBirth)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var age = today.Year - dateOfBirth.Year;
        var shift = (dateOfBirth > today.AddYears(-age)) ? -1 : 0;

        return age + shift;
    }
}
