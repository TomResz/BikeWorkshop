using BikeWorkshop.Application.Functions.DTO;
using Newtonsoft.Json;

namespace BikeWorkshop.API.Tests.Extensions;
public static class PagedListDeserialization
{
	public static (List<OrderDto> Items,int Page,int PageSize,int PageTotalCount,int TotalCount,
		bool HasNextPage,bool HasPreviousPage) DeserializeToOrderDto(string json)
	{
		var jsonReturn = JsonConvert.DeserializeAnonymousType(json, new
		{
			Items = new List<OrderDto>(),
			Page = 0,
			PageSize = 0,
			PageTotalCount = 0,
			TotalCount = 0,
			HasNextPage = false,
			HasPreviousPage = false
		}) ;
		var items = jsonReturn!.Items;
		var page = jsonReturn.Page;
		var pageSize = jsonReturn.PageSize;
		var pageTotalCount = jsonReturn.PageTotalCount;
		var totalCount = jsonReturn.TotalCount;
		var hasNextPage = jsonReturn.HasNextPage;
		var hasPreviousPage = jsonReturn.HasPreviousPage;

		return (items,page,pageSize,pageTotalCount,totalCount,hasNextPage,hasPreviousPage);
	}
}
