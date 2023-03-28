using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="Queryable" /> objects.
/// </summary>
[PublicAPI]
public static class QueryableExtensions
{
    /// <summary>
    ///     Returns a specified page of elements from the source sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="source">The source sequence to page.</param>
    /// <param name="pageNumber">The index of the page to retrieve, starting from 1.</param>
    /// <param name="pageSize">The number of elements to include in the page.</param>
    /// <returns>
    ///     An <see cref="System.Linq.IQueryable{T}" /> that represents the specified page of elements from the source
    ///     sequence.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="source" /> is null.</exception>
    /// <exception cref="System.ArgumentOutOfRangeException">
    ///     Thrown when <paramref name="pageNumber" /> or
    ///     <paramref name="pageSize" /> is less than or equal to 0.
    /// </exception>
    /// <remarks>
    ///     This extension method can be used to page any type that implements the <see cref="System.Linq.IQueryable{T}" />
    ///     interface, such as a database table.
    ///     The <paramref name="pageNumber" /> parameter is one-based, meaning that the first page is 1, not 0.
    ///     The returned sequence is an <see cref="System.Linq.IQueryable{T}" /> that represents the specified page of elements
    ///     from the source sequence.
    /// </remarks>
    /// <example>
    ///     The following code demonstrates how to use the <see cref="Page{T}(IQueryable{T}, int, int)" /> method to retrieve a
    ///     page of elements from a database table.
    ///     <code><![CDATA[
    /// using (var dbContext = new MyDbContext())
    /// {
    ///     var query = dbContext.Customers.OrderBy(c => c.LastName);
    ///     
    ///     var pageSize = 10;
    ///     var pageNumber = 2;
    ///     
    ///     var pageOfCustomers = query.Page(pageNumber, pageSize);
    ///     foreach (var customer in pageOfCustomers)
    ///     {
    ///         Console.WriteLine("{0} {1}", customer.FirstName, customer.LastName);
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    public static IQueryable<T> Page<T>(this IQueryable<T> source, int pageNumber, int pageSize)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source), $"The {nameof(source)} list to be paged cannot be null.");
        }

        if (pageNumber <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pageNumber),
                $"The {nameof(pageNumber)} cannot be less than or equal to 0.");
        }

        if (pageSize <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize),
                $"The {nameof(pageSize)} cannot be less than or equal to 0.");
        }

        var skipCount = (pageNumber - 1) * pageSize;
        source = source.Skip(skipCount).Take(pageSize);

        return source;
    }
}