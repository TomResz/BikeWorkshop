using BikeWorkshop.API.Tests.Settings.BaseClasses;
using BikeWorkshop.API.Tests.Settings.WebAppFactories;
using BikeWorkshop.Application.Functions.DTO;
using FluentAssertions;
using System.Text.Json;

namespace BikeWorkshop.API.Tests.ServiceToOrder;
public class GetByOrderIdServiceToOrderTests
    : ManagerBaseClass
{
    private Func<Guid, string> _path = (id) => $"api/servicetoorder/{id}/all";
    public GetByOrderIdServiceToOrderTests(ManagerTestWebApplicationFactory<Program> managerTestWeb) : base(managerTestWeb)
    {
    }

    [Fact]
    public async Task GetByOrderId_UnknownOrder_ReturnsNotFoundStatus()
    {
        var id = Guid.NewGuid();

        var response = await httpClient.GetAsync(_path(id));

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetByOrderId_KnownOrder_ReturnsOkStatus()
    {
        var servicesOrders = await dbContext.AddServicesToOrders();
        var id = servicesOrders.Select(x=>x.OrderId).FirstOrDefault();

        var response = await httpClient.GetAsync(_path(id));
        var dtos = JsonSerializer.Deserialize<List<ServiceToOrderDto>>(await response.Content.ReadAsStringAsync());


        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        dtos!.Count.Should().Be(servicesOrders.Count());
        dtos!.Should().BeInAscendingOrder(x => x.Price);

    }
}
