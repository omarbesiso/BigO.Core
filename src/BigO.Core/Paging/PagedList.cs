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
    ///     Creates an empty instance of a paged list for the specified type, utilizing a cache to improve performance.
    /// </summary>
    /// <typeparam name="TPagedList">
    ///     The concrete type of the paged list to be instantiated. This type must inherit from
    ///     <see cref="PagedList{TItem}" /> and have a constructor that matches the expected signature (
    ///     <see cref="IEnumerable{TItem}" />, int, int, int).
    /// </typeparam>
    /// <typeparam name="TItem">The type of items contained in the paged list. Must be a class.</typeparam>
    /// <returns>An empty instance of <typeparamref name="TPagedList" />.</returns>
    /// <exception cref="System.MissingMethodException">
    ///     Thrown when no matching constructor is found for the specified
    ///     <typeparamref name="TPagedList" />.
    /// </exception>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown if <typeparamref name="TPagedList" /> cannot be instantiated
    ///     due to it being abstract, an interface, lacking an accessible constructor, or the type name being null.
    /// </exception>
    /// <remarks>
    ///     This method leverages a static cache (<see cref="PagedListCache" />) to store and reuse empty instances of paged
    ///     lists, significantly reducing the overhead associated with dynamic instance creation via reflection for frequently
    ///     requested types. It dynamically instantiates a paged list of the specified type with default parameters, assuming
    ///     the presence of a constructor in <typeparamref name="TPagedList" /> that accepts parameters for a
    ///     <see cref="List{TItem}" />, total count, page number, and page size. This approach is ideal for methods that need
    ///     to return an empty paged result set without specific knowledge of the paged list's implementation being used.
    ///     Due to the caching mechanism, subsequent requests for the same <typeparamref name="TPagedList" /> type will receive
    ///     a cached instance, enhancing performance, especially in scenarios with frequent requests for empty paged lists.
    ///     Usage example:
    ///     <code>
    /// var emptyPagedList = PagedList&lt;MyPagedList, MyItem&gt;.CreateEmpty&lt;MyPagedList, MyItem&gt;();
    /// </code>
    ///     It is important to ensure thread safety and manage the lifetime of objects within the cache properly, especially in
    ///     long-running applications to prevent memory leaks or stale data. However, as each cached instance is empty and
    ///     immutable, the risk of stale data in this specific context is minimal.
    /// </remarks>
    public static TPagedList CreateEmpty<TPagedList, TItem>() where TPagedList : PagedList<TItem>, new() where TItem : class
    {
        return PagedListCache.CreateEmpty<TPagedList, TItem>();
    }
}