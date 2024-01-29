namespace BikeWorkshop.Application.Pagination;

public class PagedList<T>
{
	public List<T> Items { get; private set; }
	public int Page { get; private set; }
	public int PageSize { get; private set; }
	public int PageTotalCount => TotalCount != 0
		? (TotalCount + PageSize - 1) / PageSize
		: 1;
	public int TotalCount { get; private set; }
	public bool HasNextPage => Page * PageSize < TotalCount;
	public bool HasPreviousPage => Page > 1;

	private PagedList(List<T> items, int page, int pageSize, int totalCount)
	{
		Items = items;
		Page = page;
		PageSize = pageSize;
		TotalCount = totalCount;
	}
	public static PagedList<T> Create(List<T> list, int page, int pageSize)
	{
		var totalCount = list.Count();
		var items = list
			.Skip((page - 1) * pageSize)
			.Take(pageSize)
			.ToList();
		return new(items, page, pageSize, totalCount);
	}
}
