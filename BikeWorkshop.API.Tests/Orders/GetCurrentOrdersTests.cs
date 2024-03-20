using BikeWorkshop.API.Tests.Settings.BaseClasses;
using BikeWorkshop.API.Tests.Settings.DatabaseFilters;
using BikeWorkshop.API.Tests.Settings.WebAppFactories;
using BikeWorkshop.Application.Functions.DTO;
using FluentAssertions;
using System.Text.Json;

namespace BikeWorkshop.API.Tests.Orders;
public class GetCurrentOrdersTests
	: ManagerBaseClass
{
	public GetCurrentOrdersTests(ManagerTestWebApplicationFactory<Program> managerTestWeb) : base(managerTestWeb)
	{
	}

	[Theory]
	[InlineData("ascda")]
	[InlineData("descd")]
	public async Task GetAllCurrent_InvalidSortingParameter_ReturnsBadRequestStatus(string parameter)
	{
		var response = await httpClient.GetAsync($"api/order/current/{parameter}");
		response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task GetAllCurrent_DescendingSortingOrder_ReturnsOkStatus()
	{
		await dbContext.Initialize();
		await dbContext.AddOrdersWithCurrentStatus();

		var response = await httpClient.GetAsync("api/order/current/desc");
		var orders = JsonSerializer.Deserialize<List<OrderDto>>(await response.Content.ReadAsStringAsync());	

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
		
		orders.Should().BeInDescendingOrder(x => x.AddedDate);
	}

	[Fact]
	public async Task GetAllCurrent_AscendingSortingOrder_ReturnsOkStatus()
	{
		await dbContext.Initialize();
		await dbContext.AddOrdersWithCurrentStatus();

		var response = await httpClient.GetAsync("api/order/current/asc");
		var orders = JsonSerializer.Deserialize<List<OrderDto>>(await response.Content.ReadAsStringAsync());

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

		orders.Should().BeInAscendingOrder(x => x.AddedDate);
	}

}
