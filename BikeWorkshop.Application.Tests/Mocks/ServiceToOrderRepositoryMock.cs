using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Shared.Exceptions;
using Moq;

namespace BikeWorkshop.Application.Tests.Mocks;

internal class ServiceToOrderRepositoryMock
{
	public static List<ServiceToOrder> ServiceToOrders => ServiceToOrderList();
	
	public static Order Order => new Order() 
	{ 
		Id=Guid.NewGuid(),
		AddedDate = DateTime.Now,
		Description = "Description",
		ShortUniqueId = "123456",
	};

	public static Order SecondOrder => new Order()
	{
		Id = Guid.NewGuid(),
		AddedDate = DateTime.Now,
		Description = "Description of second order",
		ShortUniqueId = "1234567",
	};
	public static Service Service => new Service()
	{
		Id = Guid.NewGuid(),	
		Name = "Name",
	};


	public static Mock<IServiceToOrderRepository> GetMockedRepo()
	{
		var list = ServiceToOrderList();
		var mock = new Mock<IServiceToOrderRepository>();
		mock.Setup(x => x.GetById(It.IsAny<Guid>()))
			.ReturnsAsync( (Guid id) =>
			{
				var result = list.FirstOrDefault(x => x.Id == id);
				return result;
			});
		mock.Setup(x => x.GetByOrderId(It.IsAny<Guid>()))
			.ReturnsAsync((Guid orderId) =>
			{
				var result = list.Where(x => x.OrderId == orderId).ToList();
				return result;
			});
		mock.Setup(x => x.Add(It.IsAny<ServiceToOrder>()))
			.Callback<ServiceToOrder>(x =>
		{
			list.Add(x);
		});
		mock.Setup(x => x.Delete(It.IsAny<ServiceToOrder>()))
			.Callback<ServiceToOrder>(x =>
			{
				if(list.Contains(x)) 
					list.Remove(x);
				else throw new NotFoundException("Not found!");
			});
		return mock;	
	}
	private static List<ServiceToOrder> ServiceToOrderList()
	{
		var list = new List<ServiceToOrder>();
		var order = new Order(); 
		for(int i=0; i< 100; i++)
		{
			if(i > 50)
			{
				order = Order;
			}
			else
			{
				order = SecondOrder;
			}

			list.Add(new ServiceToOrder()
			{
				Id = Guid.NewGuid(),
				Count = i,
				Price = (decimal)i + 25m,
				Order = order,
				OrderId = order.Id,
				Service = Service,
				ServiceId = Service.Id
			});
		}
		return list;
	}
	
}
