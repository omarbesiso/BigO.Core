using System.Collections.Concurrent;

namespace BigO.Core.Paging;

internal static class PagedListCache
{
    internal static readonly ConcurrentDictionary<string, object> EmptyPagedListsCache = new();

    internal static TPagedList CreateEmpty<TPagedList, TItem>() where TPagedList : PagedList<TItem, TPagedList>
    {
        const int totalCount = 0;
        const int pageNumber = 1;
        const int pageSize = 1;
        var emptyPagedList = EmptyPagedListsCache.GetOrAdd(typeof(TPagedList).FullName ?? throw new InvalidOperationException(), _ => (TPagedList)Activator.CreateInstance(typeof(TPagedList), new List<TItem>(), totalCount, pageNumber, pageSize)!);
        return (TPagedList)emptyPagedList;
    }
}