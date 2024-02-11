using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Infrastructure.EF.Context;
using Microsoft.EntityFrameworkCore;

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

	public async Task Delete(ServiceToOrder serviceToOrder)
	{
		_dbContext.ServiceToOrders.Remove(serviceToOrder);
		await _dbContext.SaveChangesAsync();
	}

	public async Task<ServiceToOrder?> GetById(Guid serviceToOrderId)
		=> await _dbContext
		.ServiceToOrders
		.FirstOrDefaultAsync(x=>x.Id == serviceToOrderId);

	public async Task<List<ServiceToOrder>> GetByOrderId(Guid orderId)
		=> await _dbContext
		.ServiceToOrders
		.Where(x=>x.OrderId == orderId)
		.AsNoTracking()
		.ToListAsync();

	public async Task<List<ServiceToOrder>> GetServiceDetailsByOrderId(Guid orderId)
		=> await _dbContext
		.ServiceToOrders
		.AsNoTracking()
		.Include(x => x.Service)
		.Where(x => x.OrderId == orderId)
		.ToListAsync();
}
