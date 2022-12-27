using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="Queryable" /> objects.
/// </summary>
[PublicAPI]
public static class QueryableExtensions
{
    /// <summary>
    ///     Returns a specified number of contiguous elements from the start of a sequence.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source" />.</typeparam>
    /// <param name="source">The <see cref="IQueryable{T}" /> to return elements from.</param>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of elements to retrieve in each page.</param>
    /// <returns>
    ///     An <see cref="IQueryable{T}" /> that contains the specified number of elements from the start of the input
    ///     sequence.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If <paramref name="source" /> is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method applies the <see cref="IQueryable{T}.Skip(int)" /> and <see cref="IQueryable{T}.Take(int)" /> methods
    ///     to the input
    ///     <paramref name="source" /> to implement paging. The <paramref name="pageNumber" /> parameter determines the number
    ///     of elements to skip,
    ///     and the <paramref name="pageSize" /> parameter determines the number of elements to take.
    /// </remarks>
    public static IQueryable<T> Page<T>(this IQueryable<T> source, int pageNumber, int pageSize)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source), $"The {nameof(source)} list to be paged cannot be null.");
        }

        var skipCount = (pageNumber - 1) * pageSize;
        source = source.Skip(skipCount).Take(pageSize);

        return source;
    }
}