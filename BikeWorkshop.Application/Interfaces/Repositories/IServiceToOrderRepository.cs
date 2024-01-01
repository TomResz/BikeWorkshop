using BikeWorkshop.Domain.Entities;

namespace BikeWorkshop.Application.Interfaces.Repositories;

public interface IServiceToOrderRepository
{
	Task Add(ServiceToOrder serviceToOrder);
}
