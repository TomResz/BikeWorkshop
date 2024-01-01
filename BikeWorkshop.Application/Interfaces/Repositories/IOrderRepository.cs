using BikeWorkshop.Domain.Entities;

namespace BikeWorkshop.Application.Interfaces.Repositories;

public interface IOrderRepository
{
	Task Add(Order order);
	Task<List<Order>> GetAllRetrieved();
	Task<List<Order>> GetAllCompleted();
	Task<List<Order>> GetAllActive();
	Task<Order?> GetById(Guid orderId);
}
