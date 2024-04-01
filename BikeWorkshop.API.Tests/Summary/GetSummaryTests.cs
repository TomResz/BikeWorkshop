using BikeWorkshop.API.Tests.Settings.BaseClasses;
using BikeWorkshop.API.Tests.Settings.WebAppFactories;
using FluentAssertions;

namespace BikeWorkshop.API.Tests.Summary;
public class GetSummaryTests : ManagerBaseClass
{
    private const string _path = "api/summary";

    public GetSummaryTests(ManagerTestWebApplicationFactory<Program> managerTestWeb) 
        : base(managerTestWeb)
    {
    }

    [Fact]
    public async Task Get_UnknownOrderWithSummary_ReturnsNotFoundStatus()
    {
        var id = Guid.NewGuid();

        var response = await httpClient.GetAsync($"{_path}/{id}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_KnownOrderWithSummary_ReturnsOkStatus()
    {
        var summary = await dbContext.AddSummary();
        var id = summary.OrderId;    

        var response = await httpClient.GetAsync($"{_path}/{id}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
}
