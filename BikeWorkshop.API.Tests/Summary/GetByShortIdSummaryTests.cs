using BikeWorkshop.API.Tests.Settings.BaseClasses;
using BikeWorkshop.API.Tests.Settings.WebAppFactories;
using FluentAssertions;

namespace BikeWorkshop.API.Tests.Summary;
public class GetByShortIdSummaryTests : ManagerBaseClass
{
    private const string _path = "api/summary/short";
    public GetByShortIdSummaryTests(ManagerTestWebApplicationFactory<Program> managerTestWeb) : base(managerTestWeb)
    {
    }

    [Fact]
    public async Task GetByShortId_UnknownOrderSummary_ReturnsNotFoundStatus()
    {
        string uniqueId = "Unique-Id";

        var response = await httpClient.GetAsync($"{_path}/{uniqueId}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetByShortId_EmptyId_ReturnsNotFoundStatus()
    {

        var response = await httpClient.GetAsync($"{_path}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetByShortId_KnownOrderSummary_ReturnsNotFoundStatus()
    {
        var summary = await dbContext.AddSummary();
        var id = summary.Order.ShortUniqueId;

        var response = await httpClient.GetAsync($"{_path}/{id}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
}
