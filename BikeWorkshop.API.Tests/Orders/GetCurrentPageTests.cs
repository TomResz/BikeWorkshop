using BikeWorkshop.API.Tests.Extensions;
using BikeWorkshop.API.Tests.Settings.BaseClasses;
using BikeWorkshop.API.Tests.Settings.DatabaseFilters;
using BikeWorkshop.API.Tests.Settings.WebAppFactories;
using FluentAssertions;

namespace BikeWorkshop.API.Tests.Orders;
public class GetCurrentPageTests
	: ManagerBaseClass
{
	public GetCurrentPageTests(ManagerTestWebApplicationFactory<Program> managerTestWeb)
		: base(managerTestWeb)
	{
	}

	[Theory]
	[InlineData(1, 5)]
	[InlineData(2, 5)]
	public async Task GetCurrentPage_CorrectParameters_ReturnsOkStatus(int page, int pageSize)
	{
		await dbContext.Initialize();
		var orders = await dbContext.AddOrdersWithCurrentStatus(); // 30 items 

		var response = await httpClient.GetAsync($"api/order/current?page={page}&pageSize={pageSize}");
		var jsonResponse = await response.Content.ReadAsStringAsync();

		var pagedList = PagedListDeserialization.DeserializeToOrderDto(jsonResponse);
		pagedList.Items?.Count().Should().Be(pageSize);
		pagedList.Page.Should().Be(page);
		pagedList.TotalCount.Should().Be(orders.Count());
		response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
	}

	[Theory]
	[InlineData(0, 5)]
	[InlineData(2, 0)]
	public async Task GetCurrentPage_InvalidParameters_ReturnsBadRequestStatus(int page, int pageSize)
	{
		await dbContext.Initialize();
		await dbContext.AddOrdersWithCurrentStatus();

		var response = await httpClient.GetAsync($"api/order/current?page={page}&pageSize={pageSize}");

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task GetCurrentPage_MissingParameters_ReturnsBadRequestStatus()
	{
		await dbContext.Initialize();
		await dbContext.AddOrdersWithCurrentStatus();

		var firstResponse = await httpClient.GetAsync($"api/order/current?page=10");
		var secondResponse = await httpClient.GetAsync($"api/order/current?pageSize=10");

		firstResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
		secondResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
	}
}
