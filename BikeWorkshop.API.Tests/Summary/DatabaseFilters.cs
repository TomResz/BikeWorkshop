using BikeWorkshop.API.Tests.ServiceToOrder;
using BikeWorkshop.Domain.Enums;
using BikeWorkshop.Infrastructure.EF.Context;
using Microsoft.EntityFrameworkCore;

namespace BikeWorkshop.API.Tests.Summary;
public static partial class DatabaseFilters
{
    public async static Task<Domain.Entities.Summary> AddSummary(this BikeWorkshopDbContext context)
    {
        var serviceToOrders = await context.AddServicesToOrders(); // total price 200
        
        var orderId = serviceToOrders.Select(x => x.OrderId).First();

        var order = await context.Orders.FirstAsync(x=> x.Id == orderId);

        order.OrderStatusId = (int)Status.Completed;

        context.Orders.Update(order);

        var summary = new Domain.Entities.Summary
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            TotalPrice = 200m,
            EndedDate = DateTime.UtcNow,
            Conclusion = "Example-conclusions",
        };
        await context.AddAsync(summary);
        await context.SaveChangesAsync();

        return await context
            .Summaries
            .Include(x => x.Order)
            .FirstAsync();
    }
    public async static Task<Domain.Entities.Summary> AddSummaryWithRetrievedStatus(this BikeWorkshopDbContext context)
    {
        var serviceToOrders = await context.AddServicesToOrders(); // total price 200

        var orderId = serviceToOrders.Select(x => x.OrderId).First();

        var order = await context.Orders.FirstAsync(x => x.Id == orderId);

        order.OrderStatusId = (int)Status.Retrieved;

        context.Orders.Update(order);

        var summary = new Domain.Entities.Summary
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            TotalPrice = 200m,
            EndedDate = DateTime.UtcNow,
            RetrievedDate = DateTime.UtcNow,
            Conclusion = "Example-conclusions",
        };
        await context.AddAsync(summary);
        await context.SaveChangesAsync();

        return await context
            .Summaries
            .Include(x => x.Order)
            .FirstAsync();
    }
}
