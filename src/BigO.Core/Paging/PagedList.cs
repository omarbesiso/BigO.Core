using System.Text.Json.Serialization;
using BigO.Core.Validation;

namespace BigO.Core.Paging;

/// <summary>
///     Represents a paged list of items.
/// </summary>
/// <typeparam name="T">The type of the items in the list.</typeparam>
[PublicAPI]
public abstract record PagedList<T> : IPagedList<T>
    where T : class
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="PagedList{T}" /> class.
    /// </summary>
    /// <param name="items">The items for the current page.</param>
    /// <param name="totalCount">The total number of items across all pages.</param>
    /// <param name="pageNumber">The current page number.</param>
    /// <param name="pageSize">The size of the page.</param>
    protected PagedList(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Guard.Minimum(totalCount, 0);
        Guard.Minimum(pageNumber, 1);
        Guard.Minimum(pageSize, 1);

        TotalCount = totalCount;
        PageSize = pageSize;
        PageNumber = pageNumber;
        Items = new List<T>(items);

        if (totalCount > 0 && pageSize > 0)
        {
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
        else
        {
            TotalPages = 0;
        }

        HasPrevious = PageNumber > 1;
        HasNext = PageNumber < TotalPages;
    }

    /// <summary>
    ///     Gets the items for the current page.
    /// </summary>
    [JsonPropertyOrder(10)]
    [JsonPropertyName("items")]
    public IReadOnlyList<T> Items { get; }

    /// <summary>
    ///     Gets the current page number.
    /// </summary>
    [JsonPropertyOrder(20)]
    [JsonPropertyName("currentPage")]
    public int PageNumber { get; private set; }

    /// <summary>
    ///     Gets the total number of pages.
    /// </summary>
    [JsonPropertyOrder(30)]
    [JsonPropertyName("totalPages")]
    public int TotalPages { get; private set; }

    /// <summary>
    ///     Gets the size of the page.
    /// </summary>
    [JsonPropertyOrder(40)]
    [JsonPropertyName("pageSize")]
    public int PageSize { get; private set; }

    /// <summary>
    ///     Gets the total number of items across all pages.
    /// </summary>
    [JsonPropertyOrder(50)]
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; private set; }

    /// <summary>
    ///     Gets a value indicating whether there is a previous page.
    /// </summary>
    [JsonPropertyOrder(60)]
    [JsonPropertyName("hasPrevious")]
    public bool HasPrevious { get; private set; }

    /// <summary>
    ///     Gets a value indicating whether there is a next page.
    /// </summary>
    [JsonPropertyOrder(70)]
    [JsonPropertyName("hasNext")]
    public bool HasNext { get; private set; }

    /// <summary>
    ///     Creates an empty instance of a paged list for the specified type.
    /// </summary>
    /// <typeparam name="TPagedList">
    ///     The concrete type of the paged list to be instantiated. This type must inherit from
    ///     <see cref="PagedList{TItem}" /> and have a constructor that matches the expected signature.
    /// </typeparam>
    /// <typeparam name="TItem">The type of items contained in the paged list. Must be a class.</typeparam>
    /// <returns>An empty instance of <typeparamref name="TPagedList" />.</returns>
    /// <exception cref="System.MissingMethodException">
    ///     Thrown when no matching constructor is found for the specified
    ///     <typeparamref name="TPagedList" />.
    /// </exception>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown if <typeparamref name="TPagedList" /> cannot be instantiated.
    ///     This can occur if the type is abstract, an interface, or does not have an accessible constructor that matches the
    ///     expected signature.
    /// </exception>
    /// <remarks>
    ///     This method utilizes reflection to dynamically instantiate a paged list of the specified type. It assumes the
    ///     presence of a constructor in <typeparamref name="TPagedList" /> that accepts a <see cref="List{TItem}" />, total
    ///     count, page number, and page size as parameters. This is particularly useful for creating return types for methods
    ///     that need to provide an empty paged result set without knowing the specific implementation of
    ///     <see cref="PagedList{T}" /> being used.
    ///     The method simplifies the instantiation of paged list types, reducing boilerplate code and facilitating the
    ///     consistent handling of empty result sets across different parts of an application.
    /// </remarks>
    public static TPagedList CreateEmpty<TPagedList, TItem>() where TPagedList : PagedList<TItem> where TItem : class
    {
        return (TPagedList)Activator.CreateInstance(typeof(TPagedList), new List<TItem>(), 0, 1, 0)!;
    }
}