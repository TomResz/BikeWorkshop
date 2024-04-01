using BikeWorkshop.API.Tests.Settings.BaseClasses;
using BikeWorkshop.API.Tests.Settings.DatabaseFilters;
using BikeWorkshop.API.Tests.Settings.WebAppFactories;
using BikeWorkshop.Application.Functions.OrderFunctions.Events.CreateOrder;
using FluentAssertions;
using System.Text.Json;

namespace BikeWorkshop.API.Tests.Orders;
public class CreateOrderTests
	: WorkerBaseClass
{
	private readonly CreateOrderEvent _command;
	public CreateOrderTests(WorkerTestWebApplicationFactory<Program> workerTestWeb) : base(workerTestWeb)
	{
		_command = new("email@email.com", "123456789","Rear shifter");
	}

	[Fact]
	public async Task CreateOrder_ValidData_ReturnsCreatedStatus()
	{
		await dbContext.Initialize();
		var json = JsonSerializer.Serialize(_command);
		var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

		var response = await httpClient.PostAsync("api/order/create", httpContent);

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
	}

	[Fact]
	public async Task CreateOrder_InvalidEmailPattern_ReturnsBadRequestStatus()
	{
		var email = "tom@tom";
		await dbContext.Initialize();
		var command = _command with { Email = email };
		var json = JsonSerializer.Serialize(command);
		var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

		var response = await httpClient.PostAsync("api/order/create", httpContent);

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
	}


	[Fact]
	public async Task CreateOrder_InvalidPhoneNumberPattern_ReturnsBadRequestStatus()
	{
		var phoneNumber = "12345678a";
		await dbContext.Initialize();
		var command = _command with { PhoneNumber = phoneNumber };
		var json = JsonSerializer.Serialize(command);
		var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

		var response = await httpClient.PostAsync("api/order/create", httpContent);

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task CreateOrder_EmptyDescription_ReturnsBadRequestStatus()
	{
		await dbContext.Initialize();
		var command = _command with { Description = "" };
		var json = JsonSerializer.Serialize(command);
		var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

		var response = await httpClient.PostAsync("api/order/create", httpContent);

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
	}
}
