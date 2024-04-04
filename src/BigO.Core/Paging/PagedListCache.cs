using System.Collections.Concurrent;

namespace BigO.Core.Paging;

internal static class PagedListCache
{
    internal static readonly ConcurrentDictionary<string, object> EmptyPagedListsCache = new();

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
        var emptyPagedList = EmptyPagedListsCache.GetOrAdd(
            typeof(TPagedList).FullName ?? throw new InvalidOperationException(),
            _ => (TPagedList)Activator.CreateInstance(typeof(TPagedList), new List<TItem>(), 0, 1, 0)!);
        return (TPagedList)emptyPagedList;
    }
}