namespace BigO.Core.Paging;

public static class PagedListExtensions
{
    public static TPagedList CreateEmpty<TPagedList, TItem>() where TPagedList : PagedList<TItem> where TItem : class
    {
        return (TPagedList)Activator.CreateInstance(typeof(TPagedList), new List<TItem>(), 0, 1, 0)!;
    }
}