using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Infrastructure.EF.Context;

namespace BikeWorkshop.Infrastructure.EF.Repositories;

public class ServiceToOrderRepository
	: IServiceToOrderRepository
{
	private readonly BikeWorkshopDbContext _dbContext;

	public ServiceToOrderRepository(BikeWorkshopDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task Add(ServiceToOrder serviceToOrder)
	{
		await _dbContext.ServiceToOrders.AddAsync(serviceToOrder);	
		await _dbContext.SaveChangesAsync();
	}
}
