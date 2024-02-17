using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Domain.Enums;
using BikeWorkshop.Infrastructure.EF.Context;
using Microsoft.EntityFrameworkCore;

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
	public async Task<List<Order>> GetAllActive()
		=> await _context
			.Orders
			.Where(x => x.OrderStatusId == (int)Status.During)
			.AsNoTracking()
			.ToListAsync();

	public async Task<List<Order>> GetAllCompleted()
		=> await _context
			.Orders
			.Where(x => x.OrderStatusId == (int)Status.Completed)
			.AsNoTracking()
			.ToListAsync();
	public async Task<List<Order>> GetAllRetrieved()
		=> await _context
			.Orders
			.Where(x => x.OrderStatusId == (int)Status.Retrieved)
			.AsNoTracking()
			.ToListAsync();

	public async Task<Order?> GetById(Guid orderId)
		=> await _context
		.Orders
		.FirstOrDefaultAsync(x=>x.Id == orderId);

	public async Task Update(Order order)
	{
		_context.Orders.Update(order);
		await _context.SaveChangesAsync();
	}
}
