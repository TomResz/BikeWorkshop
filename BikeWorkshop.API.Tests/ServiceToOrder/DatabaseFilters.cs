using BikeWorkshop.API.Tests.Orders;
using BikeWorkshop.API.Tests.Service;
using BikeWorkshop.API.Tests.Settings.DatabaseFilters;
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
}
