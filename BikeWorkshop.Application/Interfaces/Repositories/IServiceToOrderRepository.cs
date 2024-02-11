using BikeWorkshop.Domain.Entities;

namespace BikeWorkshop.Application.Interfaces.Repositories;

public interface IServiceToOrderRepository
{
	Task Add(ServiceToOrder serviceToOrder);
	Task Delete(ServiceToOrder serviceToOrder);
	Task<ServiceToOrder?> GetById(Guid serviceToOrderId);
	Task<List<ServiceToOrder>> GetByOrderId(Guid orderId);
	Task<List<ServiceToOrder>> GetServiceDetailsByOrderId(Guid orderId);
}
