using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Contains useful utility/extensions methods for working with <see cref="IQueryable" /> objects.
/// </summary>
[PublicAPI]
public static class QueryableExtensions
{
    /// <summary>
    ///     Returns a specified number of contiguous elements after bypassing a specified number of elements in a sequence and
    ///     then returns the remaining elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in <paramref name="source" />.</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}" /> to return elements from.</param>
    /// <param name="pageNumber">The number of the page to return the elements of.</param>
    /// <param name="pageSize">The number of elements to return per page.</param>
    /// <returns>An <see cref="IQueryable{T}" /> that contains the specified number of elements specified.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="source" /> is <c>null</c>.</exception>
    public static IQueryable<T> Page<T>(this IQueryable<T> source, int pageNumber, int pageSize)
    {
        ArgumentNullException.ThrowIfNull(source);

        var skipCount = (pageNumber - 1) * pageSize;
        source = source.Skip(skipCount).Take(pageSize);

        return source;
    }
}