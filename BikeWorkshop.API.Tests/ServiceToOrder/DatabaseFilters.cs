using BikeWorkshop.API.Tests.Orders;
using BikeWorkshop.API.Tests.Service;
using BikeWorkshop.API.Tests.Settings.DatabaseFilters;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Domain.Enums;
using BikeWorkshop.Infrastructure.EF.Context;
using Microsoft.EntityFrameworkCore;

namespace BikeWorkshop.API.Tests.ServiceToOrder;
public static partial class DatabaseFilters
{
    public async static Task<List<Domain.Entities.ServiceToOrder>> AddServicesToOrders(this BikeWorkshopDbContext context)
    {
        await context.Initialize();
        var order = (await context.AddOrdersWithCurrentStatus()).FirstOrDefault()!;
        var services = (await context.AddServices()).Take(2).ToList();

        var serviceToOrders = new List<Domain.Entities.ServiceToOrder>();

        foreach(var service in services)
        {
            serviceToOrders.Add( new Domain.Entities.ServiceToOrder
            {
                Id = Guid.NewGuid(),
                Count = 1,
                Price = 100m,
                OrderId = order.Id,
                ServiceId = service.Id
            });
        }
        
        await context.ServiceToOrders.AddRangeAsync(serviceToOrders);
        await context.SaveChangesAsync();

        return await context
            .ServiceToOrders
            .AsNoTracking()
            .ToListAsync();
    }

    public  static async Task<Order> AddRetrievedOrderWithLinkedServices(this BikeWorkshopDbContext context)
    {
        var orderId = (await context.AddServicesToOrders())
            .Select(x => x.OrderId)
            .First();
        var order = await context.Orders.FirstAsync(x=> x.Id == orderId);
        order.OrderStatusId = (int)Status.Retrieved;
        await context.SaveChangesAsync();
        return order;
    }
    public static async Task<Order> AddCompletedOrderWithLinkedServices(this BikeWorkshopDbContext context)
    {
        var orderId = (await context.AddServicesToOrders())
            .Select(x => x.OrderId)
            .First();
        var order = await context.Orders.FirstAsync(x => x.Id == orderId);
        order.OrderStatusId = (int)Status.Completed;
        await context.SaveChangesAsync();
        return order;
    }
}
