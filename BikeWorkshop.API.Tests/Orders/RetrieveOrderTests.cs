using BikeWorkshop.API.Tests.Settings.BaseClasses;
using BikeWorkshop.API.Tests.Settings.DatabaseFilters;
using BikeWorkshop.API.Tests.Settings.WebAppFactories;
using BikeWorkshop.Application.Functions.OrderFunctions.Command.RetrieveOrder;
using FluentAssertions;
using System.Text.Json;

namespace BikeWorkshop.API.Tests.Orders;
public class RetrieveOrderTests
	: WorkerBaseClass
{
	public RetrieveOrderTests(WorkerTestWebApplicationFactory<Program> workerTestWeb) : base(workerTestWeb)
	{
	}

	[Fact]
	public async Task RetrieveOrder_UnknownOrderId_ReturnsNotFoundStatus()
	{
		var command = new RetrieveOrderCommand(Guid.NewGuid());
		var json = JsonSerializer.Serialize(command);
		var httpContent = new StringContent(json,System.Text.Encoding.UTF8,"application/json");

		var response = await httpClient.PatchAsync("api/order/retrieve-order", httpContent);

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
	}
	[Fact]
	public async Task RetrieveOrder_KnownOrderIdAndCompletedStatus_ReturnsNotContentStatus()
	{
		await dbContext.Initialize();
		var orders = await dbContext.AddOrdersWithCompletedStatus();
		var orderId = orders.Select(x=>x.Id).FirstOrDefault();


		var command = new RetrieveOrderCommand(orderId);
		var json = JsonSerializer.Serialize(command);
		var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

		var response = await httpClient.PatchAsync("api/order/retrieve-order", httpContent);

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
	}

	[Fact]
	public async Task RetrieveOrder_KnownOrderIdAndDuringStatus_ReturnsNotFoundStatus()
	{
		await dbContext.Initialize();
		var orders = await dbContext.AddOrdersWithCurrentStatus();
		var orderId = orders.Select(x => x.Id).FirstOrDefault();


		var command = new RetrieveOrderCommand(orderId);
		var json = JsonSerializer.Serialize(command);
		var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

		var response = await httpClient.PatchAsync("api/order/retrieve-order", httpContent);

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task RetrieveOrder_KnownOrderIdAndRetrievedStatus_ReturnsBadRequestStatus()
	{
		await dbContext.Initialize();
		var orders = await dbContext.AddOrdersWithRetrievedStatus();
		var orderId = orders.Select(x => x.Id).FirstOrDefault();


		var command = new RetrieveOrderCommand(orderId);
		var json = JsonSerializer.Serialize(command);
		var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

		var response = await httpClient.PatchAsync("api/order/retrieve-order", httpContent);

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
	}
}
