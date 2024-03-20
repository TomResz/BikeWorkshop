using BikeWorkshop.API.Tests.Extensions;
using BikeWorkshop.API.Tests.Settings.BaseClasses;
using BikeWorkshop.API.Tests.Settings.DatabaseFilters;
using BikeWorkshop.API.Tests.Settings.WebAppFactories;
using FluentAssertions;

namespace BikeWorkshop.API.Tests.Orders;
public class GetPageOfCompletedTests
	: ManagerBaseClass
{

	public GetPageOfCompletedTests(ManagerTestWebApplicationFactory<Program> managerTestWeb) : base(managerTestWeb)
	{
	}

	[Theory]
	[InlineData(1,5,"asc")]
	[InlineData(2,10,"desc")]
	public async Task GetPageOfCompleted_ValidParameters_Returns200Status(int page,int pageSize,string direction)
	{
		await dbContext.Initialize();
		await dbContext.AddOrdersWithCompletedStatus();
		var response = await httpClient.GetAsync($"api/order/completed?page={page}&pageSize={pageSize}&direction={direction}");
		var pagedList = PagedListDeserialization.DeserializeToOrderDto(await response.Content.ReadAsStringAsync());


		response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
		pagedList.Page.Should().Be(page);
		pagedList.PageSize.Should().Be(pageSize);

		if(direction == "asc")
			pagedList.Items.Should().BeInAscendingOrder(x => x.AddedDate);
		else
			pagedList.Items.Should().BeInDescendingOrder(x => x.AddedDate);
	}
	[Theory]
	[InlineData(0, 5)]
	[InlineData(2, 0)]
	public async Task GetPageOfCompleted_InvalidSortingParameters_Returns200Status(int page, int pageSize)
	{
		await dbContext.Initialize();
		await dbContext.AddOrdersWithCompletedStatus();
		var response = await httpClient.GetAsync($"api/order/completed?page={page}&pageSize={pageSize}&direction=asc");
		response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
	}
}
