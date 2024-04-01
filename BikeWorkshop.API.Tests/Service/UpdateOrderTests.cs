using BikeWorkshop.API.Tests.Settings.BaseClasses;
using BikeWorkshop.API.Tests.Settings.DatabaseFilters;
using BikeWorkshop.API.Tests.Settings.WebAppFactories;
using BikeWorkshop.Application.Functions.ServiceFunctions.Commands.Update;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BikeWorkshop.API.Tests.Service;
public class UpdateOrderTests
	: ManagerBaseClass
{
	private const string _path = "api/service/update";
	public UpdateOrderTests(ManagerTestWebApplicationFactory<Program> managerTestWeb)
		: base(managerTestWeb)
	{
	}

	[Fact]
	public async Task Update_UnknownOrder_ReturnsNotFoundStatus()
	{
		// empty database
		var command = new UpdateServiceCommand(Guid.NewGuid(), "Example name");
		var json = JsonSerializer.Serialize(command);
		var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

		var response = await httpClient.PutAsync(_path, httpContent);

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task Update_KnownOrder_ReturnsNoContentStatus()
	{
		await dbContext.Initialize();
		var orders = await dbContext.AddServices();
		var order = orders.FirstOrDefault()!;
		var newName = "Updated-name";
		var command = new UpdateServiceCommand(order.Id, newName);
		var json = JsonSerializer.Serialize(command);
		var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

		var response = await httpClient.PutAsync(_path, httpContent);

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
		var updatedOrder = await dbContext.Services.FirstOrDefaultAsync(x => x.Name == newName);
		updatedOrder.Should().NotBeNull();
	}
    [Fact]
    public async Task Update_NotUniqueName_ReturnsBadRequestStatus()
    {
        await dbContext.Initialize();
        var orders = await dbContext.AddServices();
        var order = orders.FirstOrDefault()!;
        var command = new UpdateServiceCommand(order.Id, order.Name);
        var json = JsonSerializer.Serialize(command);
        var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await httpClient.PutAsync(_path, httpContent);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }


    [Theory]
	[InlineData("")]
    public async Task Update_InvalidNewName_ReturnsBadRequestStatus(string newName)
    {
        await dbContext.Initialize();
        var orders = await dbContext.AddServices();
        var order = orders.FirstOrDefault()!;
        var command = new UpdateServiceCommand(order.Id, newName);
        var json = JsonSerializer.Serialize(command);
        var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await httpClient.PutAsync(_path, httpContent);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
}
