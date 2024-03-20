using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Domain.Enums;
using BikeWorkshop.Infrastructure.EF.Context;
using Microsoft.EntityFrameworkCore;

namespace BikeWorkshop.API.Tests.Orders;
public static class DatabaseFilters
{
	public static async Task<List<Order>> AddOrdersWithCurrentStatus(this BikeWorkshopDbContext context)
	{
		var orders = new List<Order>(30);
		for(int i = 0; i < 30; i++)
		{
			var clientId = Guid.NewGuid();
			orders.Add(new Order
			{
				AddedDate = DateTime.Now,
				EmployeeId = Constants.EmployeeId,
				Description = i.ToString(),
				ShortUniqueId = $"{Constants.ShortId}{i}",
				Id = Guid.NewGuid(),
				OrderStatusId = (int)Status.During,
				ClientDataId = clientId,
				ClientData = new()
				{
					Email = $"emailclient{i}@email.com",
					PhoneNumber = "444555777",
					Id = clientId,
				},
			});
		}
		await context.Orders.AddRangeAsync(orders);
		await context.SaveChangesAsync();
		return await context.Orders.Where(x => x.OrderStatusId == (int)Status.During).ToListAsync();
	}
	public static async Task<List<Order>> AddOrdersWithCompletedStatus(this BikeWorkshopDbContext context)
	{
		var orders = new List<Order>(30);
		for (int i = 0; i < 30; i++)
		{
			var clientId = Guid.NewGuid();
			var summaryId = Guid.NewGuid();
			orders.Add(new Order
			{
				AddedDate = DateTime.Now.AddMinutes(i*2),
				EmployeeId = Constants.EmployeeId,
				Description = i.ToString(),
				ShortUniqueId = $"{Constants.ShortId}{i+30+1}",
				Id = Guid.NewGuid(),
				OrderStatusId = (int)Status.Completed,
				ClientDataId = clientId,
				SummaryId = summaryId,
				ClientData = new()
				{
					Email = $"emailclient{i+30}@email.com",
					PhoneNumber = "444555777",
					Id = clientId,
				},
				Summary = new()
				{
					Conclusion = i.ToString(),
					EndedDate = DateTime.Now.AddMilliseconds(i),
					Id = summaryId,
					RetrievedDate = DateTime.Now,
					TotalPrice = 0m,
					
				}
			});
		}
		await context.Orders.AddRangeAsync(orders);
		await context.SaveChangesAsync();
		return await context.Orders.Where(x=> x.OrderStatusId == (int)Status.Completed).ToListAsync();
	}
	public static async Task<List<Order>> AddOrdersWithRetrievedStatus(this BikeWorkshopDbContext context)
	{
		var orders = new List<Order>(30);
		for (int i = 0; i < 30; i++)
		{
			var clientId = Guid.NewGuid();
			var summaryId = Guid.NewGuid();
			orders.Add(new Order
			{
				AddedDate = DateTime.Now.AddMinutes(i * 2),
				EmployeeId = Constants.EmployeeId,
				Description = i.ToString(),
				ShortUniqueId = $"{Constants.ShortId}{i*60+2}",
				Id = Guid.NewGuid(),
				OrderStatusId = (int)Status.Retrieved,
				ClientDataId = clientId,
				SummaryId = summaryId,
				ClientData = new()
				{
					Email = $"emailclient{i+120}@email.com",
					PhoneNumber = "444555777",
					Id = clientId,
				},
				Summary = new()
				{
					Conclusion = i.ToString(),
					EndedDate = DateTime.Now.AddMinutes(i),
					Id = summaryId,
					RetrievedDate = DateTime.Now,
					TotalPrice = 0m,
				}
			});
		}
		await context.Orders.AddRangeAsync(orders);
		await context.SaveChangesAsync();
		return await context.Orders.Where(x => x.OrderStatusId == (int)Status.Retrieved).ToListAsync();
	}
}
