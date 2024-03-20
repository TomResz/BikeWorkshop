using BikeWorkshop.API.Tests.Settings.BaseClasses;
using BikeWorkshop.API.Tests.Settings.DatabaseFilters;
using BikeWorkshop.API.Tests.Settings.WebAppFactories;
using BikeWorkshop.Application.Functions.DTO;
using FluentAssertions;
using Newtonsoft.Json;

namespace BikeWorkshop.API.Tests.Orders;
public class GetByShortIdTests
	: UnauthorizedUserBaseClass
{
	public GetByShortIdTests(UnauthorizedTestWebApplicationFactory<Program> factory) : base(factory)
	{
	}

	[Fact]
	public async Task GetByShortId_UnknownShortId_ReturnsNotFoundStatus()
	{
		// empty database

		var response = await httpClient.GetAsync("api/order/search/12345");

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task GetByShortId_KnownIdAndCurrentStatus_ReturnsOkStatus()
	{
		await dbContext.Initialize();
		var orders = await dbContext.AddOrdersWithCurrentStatus();

		var shortId = orders.Select(x => x.ShortUniqueId).FirstOrDefault();

		var response = await httpClient.GetAsync($"api/order/search/{shortId}");
		var history = JsonConvert.DeserializeObject<OrderHistoryDto>(await response.Content.ReadAsStringAsync());

		history?.StatusHistoryDtos.Count().Should().Be(1);
		history?.OrderName.Should().NotBeNullOrEmpty();
		response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
	}

	[Fact]
	public async Task GetByShortId_KnownIdAndCompletedStatus_ReturnsOkStatus()
	{
		await dbContext.Initialize();
		var orders = await dbContext.AddOrdersWithCompletedStatus();

		var shortId = orders.Select(x => x.ShortUniqueId).FirstOrDefault();

		var response = await httpClient.GetAsync($"api/order/search/{shortId}");
		var history = JsonConvert.DeserializeObject<OrderHistoryDto>(await response.Content.ReadAsStringAsync());

		history?.StatusHistoryDtos.Count().Should().Be(2);
		history?.OrderName.Should().NotBeNullOrEmpty();
		response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
	}
	[Fact]
	public async Task GetByShortId_KnownIdAndRetrievedStatus_ReturnsOkStatus()
	{
		await dbContext.Initialize();
		var orders = await dbContext.AddOrdersWithRetrievedStatus();

		var shortId = orders.Select(x => x.ShortUniqueId).FirstOrDefault();

		var response = await httpClient.GetAsync($"api/order/search/{shortId}");
		var history = JsonConvert.DeserializeObject<OrderHistoryDto>(await response.Content.ReadAsStringAsync());

		history?.StatusHistoryDtos.Count().Should().Be(3);
		history?.OrderName.Should().NotBeNullOrEmpty();
		response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
	}
}
