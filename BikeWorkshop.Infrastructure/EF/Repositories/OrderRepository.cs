using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Infrastructure.EF.Context;

namespace BikeWorkshop.Infrastructure.EF.Repositories;

public class OrderRepository : IOrderRepository
{
	private readonly BikeWorkshopDbContext _context;

	public OrderRepository(BikeWorkshopDbContext context)
	{
		_context = context;
	}

	public async Task Add(Order order)
	{
		await _context
			.Orders
			.AddAsync(order);
		await _context.SaveChangesAsync();
	}
}
