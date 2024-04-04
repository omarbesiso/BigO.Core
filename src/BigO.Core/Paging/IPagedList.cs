namespace BigO.Core.Paging;

/// <summary>
///     Represents a paged list of items.
/// </summary>
/// <typeparam name="T">The type of the items in the list.</typeparam>
[PublicAPI]
public interface IPagedList<out T>
{
    /// <summary>
    ///     Gets the items for the current page.
    /// </summary>
    IReadOnlyList<T> Items { get; }

    /// <summary>
    ///     Gets the current page number.
    /// </summary>
    int PageNumber { get; }

    /// <summary>
    ///     Gets the size of the page.
    /// </summary>
    int PageSize { get; }

    /// <summary>
    ///     Gets the total number of pages.
    /// </summary>
    int TotalPages { get; }

    /// <summary>
    ///     Gets the total number of items across all pages.
    /// </summary>
    int TotalCount { get; }

    /// <summary>
    ///     Gets a value indicating whether there is a previous page.
    /// </summary>
    bool HasPrevious { get; }

    /// <summary>
    ///     Gets a value indicating whether there is a next page.
    /// </summary>
    bool HasNext { get; }
}