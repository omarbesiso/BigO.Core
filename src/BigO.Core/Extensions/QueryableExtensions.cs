using System.Linq.Expressions;
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

    /// <summary>
    /// Applies the specified predicate to the given <see cref="IQueryable{T}"/> if the given condition is <c>true</c>.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source <see cref="IQueryable{T}"/>.</typeparam>
    /// <param name="query">The source <see cref="IQueryable{T}"/> to apply the predicate to.</param>
    /// <param name="condition">A <see cref="bool"/> value indicating whether the predicate should be applied.</param>
    /// <param name="predicate">The predicate to apply if the <paramref name="condition"/> is <c>true</c>.</param>
    /// <returns>An <see cref="IQueryable{T}"/> with the predicate applied if the <paramref name="condition"/> is <c>true</c>, or the original <paramref name="query"/> if the <paramref name="condition"/> is <c>false</c>.</returns>
    /// <example>
    /// <code><![CDATA[
    /// IQueryable<int> numbers = new List<int> { 1, 2, 3, 4, 5 }.AsQueryable();
    /// bool condition = true;
    /// IQueryable<int> evenNumbers = numbers.WhereIf(condition, (number, index) => number % 2 == 0);
    /// ]]></code>
    /// </example>
    /// <remarks>
    /// The <see cref="WhereIf{T}"/> method is useful when you want to conditionally apply a predicate to an <see cref="IQueryable{T}"/> based on a boolean <paramref name="condition"/>. If the <paramref name="condition"/> is <c>true</c>, the method applies the specified <paramref name="predicate"/> to the <paramref name="query"/>. If the <paramref name="condition"/> is <c>false</c>, the method returns the original <paramref name="query"/> without applying the <paramref name="predicate"/>.
    /// </remarks>
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, int, bool>> predicate)
    {
        return condition ? query.Where(predicate) : query;
    }
}