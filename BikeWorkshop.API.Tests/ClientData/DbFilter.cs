using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Domain.Enums;
using BikeWorkshop.Infrastructure.EF.Context;

namespace BikeWorkshop.API.Tests.ClientData;
public static class DbFilter
{
	public static async Task<Order> AddOrderAndClient(this BikeWorkshopDbContext context,
		Guid orderId,Guid employeeId)
	{
		var clientId = Guid.NewGuid();
		var order = new Order
		{ 
			Id = orderId,
			Description = "Description",
			EmployeeId = employeeId,
			OrderStatusId = (int)Status.During,
			ShortUniqueId = "1234",
			AddedDate = DateTime.Now,
			ClientDataId = clientId,
			ClientData = new()
			{
				Id = clientId,
				Email = "email@email.com",
				PhoneNumber = "1234567890",
			}
		};
		await context.Orders.AddAsync(order);
		await context.SaveChangesAsync();
		return order;
	}
}
