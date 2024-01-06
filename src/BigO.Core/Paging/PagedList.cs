using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace BigO.Core.Paging;

/// <summary>
///     Represents a paged list of items.
/// </summary>
/// <typeparam name="T">The type of the items in the list.</typeparam>
/// <typeparam name="TK">The concrete paged list.</typeparam>
[PublicAPI]
[DataContract]
public abstract class PagedList<T, TK> : IPagedList<T>
    where T : class
    where TK : PagedList<T, TK>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="PagedList{T, K}" /> class.
    /// </summary>
    /// <param name="items">The items for the current page.</param>
    /// <param name="totalCount">The total number of items across all pages.</param>
    /// <param name="pageNumber">The current page number.</param>
    /// <param name="pageSize">The size of the page.</param>
    protected PagedList(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize) : this(new List<T>(items),
        totalCount, pageNumber, pageSize)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="PagedList{T, K}" /> class.
    /// </summary>
    /// <param name="items">The items for the current page.</param>
    /// <param name="totalCount">The total number of items across all pages.</param>
    /// <param name="pageNumber">The current page number.</param>
    /// <param name="pageSize">The size of the page.</param>
    protected PagedList(List<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = new List<T>(items);
        TotalCount = totalCount;
        PageSize = pageSize;
        PageNumber = pageNumber;
        TotalPages = 0;
        
        if (totalCount > 0 && pageSize > 0)
        {
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }

        HasPrevious = PageNumber > 1;
        HasNext = PageNumber < TotalPages;
    }

    /// <summary>
    ///     Gets the items for the current page.
    /// </summary>
    [JsonPropertyOrder(10)]
    [JsonPropertyName("items")]
    [DataMember(Name = "items", Order = 10)]
    public List<T> Items { get; }


    /// <summary>
    ///     Gets the current page number.
    /// </summary>
    [JsonPropertyOrder(20)]
    [JsonPropertyName("currentPage")]
    [DataMember(Name = "currentPage", Order = 20)]
    public int PageNumber { get; private set; }

    /// <summary>
    ///     Gets the total number of pages.
    /// </summary>
    [JsonPropertyOrder(30)]
    [JsonPropertyName("totalPages")]
    [DataMember(Name = "totalPages", Order = 30)]
    public int TotalPages { get; private set; }

    /// <summary>
    ///     Gets the size of the page.
    /// </summary>
    [JsonPropertyOrder(40)]
    [JsonPropertyName("pageSize")]
    [DataMember(Name = "pageSize", Order = 40)]
    public int PageSize { get; private set; }

    /// <summary>
    ///     Gets the total number of items across all pages.
    /// </summary>
    [JsonPropertyOrder(50)]
    [JsonPropertyName("totalCount")]
    [DataMember(Name = "totalCount", Order = 50)]
    public int TotalCount { get; private set; }

    /// <summary>
    ///     Gets a value indicating whether there is a previous page.
    /// </summary>
    [JsonPropertyOrder(60)]
    [JsonPropertyName("hasPrevious")]
    [DataMember(Name = "hasPrevious", Order = 60)]
    public bool HasPrevious { get; private set; }

    /// <summary>
    ///     Gets a value indicating whether there is a next page.
    /// </summary>
    [JsonPropertyOrder(70)]
    [JsonPropertyName("hasNext")]
    [DataMember(Name = "hasNext", Order = 70)]
    public bool HasNext { get; private set; }

    /// <summary>
    ///     Returns an empty paged list.
    /// </summary>
    /// <returns>An empty paged list.</returns>
    public static TK Empty => (TK)Activator.CreateInstance(typeof(TK), new List<T>(), 0, 0, 0)!;
}