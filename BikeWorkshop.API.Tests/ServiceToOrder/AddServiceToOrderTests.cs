using BikeWorkshop.API.Tests.Orders;
using BikeWorkshop.API.Tests.Service;
using BikeWorkshop.API.Tests.Settings.BaseClasses;
using BikeWorkshop.API.Tests.Settings.DatabaseFilters;
using BikeWorkshop.API.Tests.Settings.WebAppFactories;
using BikeWorkshop.Application.Functions.ServiceToOrderFunctions.Commands.Add;
using FluentAssertions;
using System.Text.Json;

namespace BikeWorkshop.API.Tests.ServiceToOrder;
public class AddServiceToOrderTests
    : ManagerBaseClass
{
    private const string _path = "api/servicetoorder/add";

    public AddServiceToOrderTests(ManagerTestWebApplicationFactory<Program> managerTestWeb)
        : base(managerTestWeb)
    {
    }

    private async Task<(Guid serviceId, Guid orderId)> InitialTestDatabase()
    {
        await dbContext.Initialize();
        var services = await dbContext.AddServices();
        var orders = await dbContext.AddOrdersWithCurrentStatus();

        var serviceId = services.Select(x => x.Id).First();
        var orderId = orders.Select(x => x.Id).First();
        return (serviceId, orderId);
    }

    [Fact]
    public async Task Add_ValidCredentials_ReturnsCreatedStatus()
    {
        var (serviceId, orderId) = await InitialTestDatabase();

        var command = new AddServiceToOrderCommand(serviceId, orderId, 1, 100m);
        var json = JsonSerializer.Serialize(command);
        var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(_path, httpContent);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-3)]
    public async Task Add_InvalidCountNumber_ReturnsBadRequestStatus(int count)
    {
        var (serviceId, orderId) = await InitialTestDatabase();

        var command = new AddServiceToOrderCommand(serviceId, orderId, count, 100m);
        var json = JsonSerializer.Serialize(command);
        var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(_path, httpContent);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(-1)]
    [InlineData(-100.25)]
    [InlineData(-3.12)]
    public async Task Add_InvalidPrice_ReturnsBadRequestStatus(decimal price)
    {
        var (serviceId, orderId) = await InitialTestDatabase();

        var command = new AddServiceToOrderCommand(serviceId, orderId, 1, price);
        var json = JsonSerializer.Serialize(command);
        var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(_path, httpContent);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Add_UnknownOrder_ReturnsNotFoundStatus()
    {
        var (serviceId, _) = await InitialTestDatabase();

        var command = new AddServiceToOrderCommand(serviceId, Guid.NewGuid(), 1, 100m);
        var json = JsonSerializer.Serialize(command);
        var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(_path, httpContent);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Add_UnknownService_ReturnsNotFoundStatus()
    {
        var (_,orderId) = await InitialTestDatabase();

        var command = new AddServiceToOrderCommand(Guid.NewGuid(), orderId, 1, 100m);
        var json = JsonSerializer.Serialize(command);
        var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(_path, httpContent);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}
