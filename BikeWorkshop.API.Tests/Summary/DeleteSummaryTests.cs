using BikeWorkshop.API.Tests.Settings.BaseClasses;
using BikeWorkshop.API.Tests.Settings.WebAppFactories;
using FluentAssertions;

namespace BikeWorkshop.API.Tests.Summary;
public class DeleteSummaryTests
    : ManagerBaseClass
{
    private const string _path = "api/summary/delete";
    public DeleteSummaryTests(ManagerTestWebApplicationFactory<Program> managerTestWeb) : base(managerTestWeb)
    {
    }

    [Fact]
    public async Task Delete_UnknownSummaryWithGivenOrderId_ReturnsNotFoundStatus()
    {
        var orderId = Guid.NewGuid();

        var response = await httpClient.DeleteAsync($"{_path}/{orderId}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_KnownSummaryWithGivenOrderId_ReturnsNoContentStatus()
    {
        var orderId = (await dbContext.AddSummary()).OrderId;

        var response = await httpClient.DeleteAsync($"{_path}/{orderId}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_RetrievedOrderStatus_ReturnsBadRequestStatus()
    {
        var orderId = (await dbContext.AddSummaryWithRetrievedStatus()).OrderId;

        var response = await httpClient.DeleteAsync($"{_path}/{orderId}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
}
