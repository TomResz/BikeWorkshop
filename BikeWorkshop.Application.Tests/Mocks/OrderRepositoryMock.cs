using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Domain.Enums;
using Moq;

namespace BikeWorkshop.Application.Tests.Mocks;

internal class OrderRepositoryMock
{
	public static Mock<IOrderRepository> GetOrderRepositoryMock()
	{
		var orders = GetOrders();
		var mock = new Mock<IOrderRepository>();
		mock.Setup(x => x.GetAllActive()).ReturnsAsync(() =>
		{
			return orders
			.Where(x=>x.OrderStatusId==(int)Status.During)
			.ToList();
		});
		mock.Setup(x => x.GetAllRetrieved()).ReturnsAsync(() =>
		{
			return orders
			.Where(x => x.OrderStatusId == (int)Status.Retrieved)
			.ToList();
		});
		mock.Setup(x => x.GetAllCompleted()).ReturnsAsync(() =>
		{
			return orders
			.Where(x => x.OrderStatusId == (int)Status.Completed)
			.ToList();
		});
		mock.Setup(x=>x.Add(It.IsAny<Order>())).Callback<Order>(
			(Order) =>
			{
				orders.Add(Order);
			});


		return mock;	
	}


	private static List<Order> GetOrders()
	{
		var orders = new List<Order>();
		orders.Add(new Order
		{
			AddedDate = DateTime.Now,
			Description = "Description",
			Id = Guid.NewGuid(),
			ShortUniqueId = "short-id",
			OrderStatusId = (int)Status.Completed
		});
		orders.Add(new Order
		{
			AddedDate = DateTime.Now,
			Description = "Description",
			Id = Guid.NewGuid(),
			ShortUniqueId = "short-id",
			OrderStatusId = (int)Status.Completed
		});
		orders.Add(new Order
		{
			AddedDate = DateTime.Now,
			Description = "Description",
			Id = Guid.NewGuid(),
			ShortUniqueId = "short-id",
			OrderStatusId = (int)Status.Completed
		});
		orders.Add(new Order
		{
			AddedDate = DateTime.Now,
			Description = "Description",
			Id = Guid.NewGuid(),
			ShortUniqueId = "short-id",
			OrderStatusId = (int)Status.Retrieved
		});
		orders.Add(new Order
		{
			AddedDate = DateTime.Now,
			Description = "Description",
			Id = Guid.NewGuid(),
			ShortUniqueId = "short-id",
			OrderStatusId = (int)Status.Retrieved
		});
		orders.Add(new Order
		{
			AddedDate = DateTime.Now,
			Description = "Description",
			Id = Guid.NewGuid(),
			ShortUniqueId = "short-id",
			OrderStatusId = (int)Status.During
		});
		orders.Add(new Order
		{
			AddedDate = DateTime.Now,
			Description = "Description",
			Id = Guid.NewGuid(),
			ShortUniqueId = "short-id",
			OrderStatusId = (int)Status.During
		});
		orders.Add(new Order
		{
			AddedDate = DateTime.Now,
			Description = "Example during",
			Id = Guid.NewGuid(),
			ShortUniqueId = "short-id",
			OrderStatusId = (int)Status.During
		});
		return orders;
	}
}
