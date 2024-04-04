using System.Text.Json.Serialization;
using BigO.Core.Validation;

namespace BigO.Core.Paging;

/// <summary>
///     Represents a paged list of items.
/// </summary>
/// <typeparam name="TItem">The type of the items in the list.</typeparam>
/// <typeparam name="TPagedList">The type of the inheriting paged list.</typeparam>
[PublicAPI]
public abstract record PagedList<TItem, TPagedList> : IPagedList<TItem> where TPagedList : PagedList<TItem, TPagedList>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="PagedList{TItem, TPagedList}" /> class.
    /// </summary>
    /// <param name="items">The items for the current page.</param>
    /// <param name="totalCount">The total number of items across all pages.</param>
    /// <param name="pageNumber">The current page number. Default is 1.</param>
    /// <param name="pageSize">The size of the page. Default is 10.</param>
    protected PagedList(IEnumerable<TItem> items, int totalCount, int pageNumber = 1, int pageSize = 10)
    {
        Guard.Minimum(totalCount, 0);
        Guard.Minimum(pageNumber, 1);
        Guard.Minimum(pageSize, 1);

        TotalCount = totalCount;
        PageSize = pageSize;
        PageNumber = pageNumber;
        Items = new List<TItem>(items);

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
    public IReadOnlyList<TItem> Items { get; }

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
    ///     Creates an empty <typeparamref name="TPagedList" /> instance with default pagination settings.
    /// </summary>
    /// <typeparam name="TItem">The type of items contained in the paged list. Must be a class.</typeparam>
    /// <typeparam name="TPagedList">
    ///     The concrete type of the paged list being instantiated, which must inherit from
    ///     <see cref="PagedList{TItem, TPagedList}" />.
    /// </typeparam>
    /// <returns>An empty instance of the specified paged list type with default pagination values.</returns>
    /// <remarks>
    ///     This method leverages a static cache to efficiently reuse instances of empty paged lists, significantly reducing
    ///     memory allocation and GC overhead for applications that frequently request empty lists. The method is thread-safe
    ///     and ensures that only one empty instance is created and cached per paged list type, enhancing application
    ///     performance.
    ///     The returned paged list instance will have the following default values:
    ///     <list type="bullet">
    ///         <item>
    ///             <description>PageNumber: 1</description>
    ///         </item>
    ///         <item>
    ///             <description>PageSize: 1</description>
    ///         </item>
    ///         <item>
    ///             <description>TotalPages: 0</description>
    ///         </item>
    ///         <item>
    ///             <description>TotalCount: 0</description>
    ///         </item>
    ///         <item>
    ///             <description>Items: An empty <see cref="IReadOnlyList{TItem}" />.</description>
    ///         </item>
    ///         <item>
    ///             <description>HasPrevious: false</description>
    ///         </item>
    ///         <item>
    ///             <description>HasNext: false</description>
    ///         </item>
    ///     </list>
    ///     Usage of this method is recommended when an API or a method needs to return an empty paged result set while still
    ///     providing clients with pagination information.
    ///     Example usage:
    ///     <code>
    /// var emptyPagedList = PagedList&lt;MyCustomPagedList, MyItem&gt;.Empty();
    /// </code>
    /// </remarks>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown if the type <typeparamref name="TPagedList" /> cannot be
    ///     instantiated. This can occur if the type is abstract, an interface, lacks a matching constructor, or if any other
    ///     error occurs during instantiation.
    /// </exception>
    public static TPagedList Empty()
    {
        return PagedListCache.CreateEmpty<TPagedList, TItem>();
    }
}