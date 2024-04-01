using BikeWorkshop.API.Tests.ServiceToOrder;
using BikeWorkshop.API.Tests.Settings.BaseClasses;
using BikeWorkshop.API.Tests.Settings.WebAppFactories;
using BikeWorkshop.Application.Functions.SummaryFunctions.Command.CreateSummaryForOrder;
using FluentAssertions;
using System.Text.Json;

namespace BikeWorkshop.API.Tests.Summary;
public class CreateSummaryTests : ManagerBaseClass
{
    private const string _path = "api/summary/create";
    public CreateSummaryTests(ManagerTestWebApplicationFactory<Program> managerTestWeb) 
        : base(managerTestWeb)
    {
    }

    [Fact]
    public async Task CreateSummary_ValidData_ReturnsCreatedStatus()
    {
        var orderId = (await dbContext.AddServicesToOrders())
            .Select(x => x.OrderId)
            .First();
        
        var command = new CreateSummaryForOrderCommand(orderId,"Example conclusions");
        var json = JsonSerializer.Serialize(command);
        var httpContent = new StringContent(json,System.Text.Encoding.UTF8,"application/json");


        var response = await httpClient.PostAsync(_path, httpContent);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
    }

    [Fact]
    public async Task CreateSummary_CompletedOrder_ReturnsBadRequestStatus()
    {
        var orderId = (await dbContext.AddCompletedOrderWithLinkedServices()).Id;

        var command = new CreateSummaryForOrderCommand(orderId, "Example conclusions");
        var json = JsonSerializer.Serialize(command);
        var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");


        var response = await httpClient.PostAsync(_path, httpContent);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateSummary_RetrievedOrder_ReturnsBadRequestStatus()
    {
        var orderId = (await dbContext.AddRetrievedOrderWithLinkedServices()).Id;

        var command = new CreateSummaryForOrderCommand(orderId, "Example conclusions");
        var json = JsonSerializer.Serialize(command);
        var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");


        var response = await httpClient.PostAsync(_path, httpContent);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }


    [Fact]
    public async Task CreateSummary_UnknownOrderId_ReturnsNotFoundStatus()
    {

        var command = new CreateSummaryForOrderCommand(Guid.NewGuid(), "Example conclusions");
        var json = JsonSerializer.Serialize(command);
        var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");


        var response = await httpClient.PostAsync(_path, httpContent);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}
