using BikeWorkshop.Domain.Entities;

namespace BikeWorkshop.Application.Interfaces.Repositories;

public interface IOrderRepository
{
	Task Add(Order order);
}
