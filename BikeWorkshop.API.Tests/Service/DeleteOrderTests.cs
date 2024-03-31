using BikeWorkshop.API.Tests.Orders;
using BikeWorkshop.API.Tests.Settings.BaseClasses;
using BikeWorkshop.API.Tests.Settings.DatabaseFilters;
using BikeWorkshop.API.Tests.Settings.WebAppFactories;
using BikeWorkshop.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BikeWorkshop.API.Tests.Service;
public class DeleteOrderTests
    : ManagerBaseClass
{
    private const string _path = "api/service/delete";
    public DeleteOrderTests(ManagerTestWebApplicationFactory<Program> managerTestWeb) : base(managerTestWeb)
    {
    }

    [Fact]
    public async Task Delete_KnownService_ReturnsNoContent()
    {
        await dbContext.Initialize();
        var services = await dbContext.AddServices();
        var serviceId = services.Select(x => x.Id).LastOrDefault();


        var response = await httpClient.DeleteAsync($"{_path}/{serviceId}");
        var service = await dbContext.Services.FirstOrDefaultAsync(x => x.Id == serviceId);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        Assert.Null(service);
    }


    [Fact]
    public async Task Delete_UnknownService_ReturnsNoFoundStatus()
    {
        var serviceId = Guid.NewGuid();

        var response = await httpClient.DeleteAsync($"{_path}/{serviceId}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }


    [Fact]
    public async Task Delete_LinkedService_ReturnsBadRequestStatus()
    {
        await dbContext.Initialize();
        var services = await dbContext.AddServices();
        var orders = await dbContext.AddOrdersWithCurrentStatus();

        var service = services.FirstOrDefault()!;
        var order = orders.FirstOrDefault()!;


        var serviceToOrder = new Domain.Entities.ServiceToOrder
        {
            Id = service.Id,
            Count = 1,
            Price = 100m,
            OrderId = order.Id,
            ServiceId = service.Id,
        };
        await dbContext.ServiceToOrders.AddAsync(serviceToOrder);
        await dbContext.SaveChangesAsync();

        var response = await httpClient.DeleteAsync($"{_path}/{service.Id}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
}
