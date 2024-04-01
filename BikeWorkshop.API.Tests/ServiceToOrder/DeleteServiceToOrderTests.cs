using BikeWorkshop.API.Tests.Settings.BaseClasses;
using BikeWorkshop.API.Tests.Settings.WebAppFactories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BikeWorkshop.API.Tests.ServiceToOrder;
public class DeleteServiceToOrderTests
    : ManagerBaseClass
{
    private Func<Guid,string> _path = (id) => $"api/servicetoorder/delete/{id}";
    public DeleteServiceToOrderTests(ManagerTestWebApplicationFactory<Program> managerTestWeb) 
        : base(managerTestWeb)
    {
    }

    [Fact]
    public async Task Delete_KnownId_ReturnsNoContentStatus()
    {
        var id = Guid.NewGuid();

        var response = await httpClient.DeleteAsync(_path(id));

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task Delete_UnknownId_ReturnsNotFoundStatus()
    {
        var servicesToOrder = await dbContext.AddServicesToOrders();
        var exampleServiceToOrderId = servicesToOrder.Select(x => x.Id).FirstOrDefault();

        var response = await httpClient.DeleteAsync(_path(exampleServiceToOrderId));
        var serviceToOrder = await dbContext
            .ServiceToOrders
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == exampleServiceToOrderId);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        Assert.Null(serviceToOrder);
    }
}
