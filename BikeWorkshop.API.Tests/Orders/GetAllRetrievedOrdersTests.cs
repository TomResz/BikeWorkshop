using BikeWorkshop.API.Tests.Settings.BaseClasses;
using BikeWorkshop.API.Tests.Settings.DatabaseFilters;
using BikeWorkshop.API.Tests.Settings.WebAppFactories;
using BikeWorkshop.Application.Functions.DTO;
using FluentAssertions;
using Newtonsoft.Json;

namespace BikeWorkshop.API.Tests.Orders;
public class GetAllRetrievedOrdersTests
	: ManagerBaseClass
{
	public GetAllRetrievedOrdersTests(ManagerTestWebApplicationFactory<Program> managerTestWeb) : base(managerTestWeb)
	{
	}

	[Theory]
	[InlineData("asc")]
	[InlineData("desc")]
	public async Task GetAllRetrieved_ValidSortingParameter_Returns200Status(string direction)
	{
		await dbContext.Initialize();
		await dbContext.AddOrdersWithRetrievedStatus();


		var response = await httpClient.GetAsync($"api/order/retrieved/{direction}");
		var orders = JsonConvert.DeserializeObject<List<OrderDto>>(await response.Content.ReadAsStringAsync());

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
		if (direction is "asc")
			orders.Should().BeInAscendingOrder(x => x.AddedDate);
		else 
			orders.Should().BeInDescendingOrder(x => x.AddedDate);
	}
	[Fact]
	public async Task GetAllRetrieved_MissingParameter_ReturnsBadRequestStatus()
	{
		var response = await httpClient.GetAsync("api/order/retrieved");

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
	}
}
