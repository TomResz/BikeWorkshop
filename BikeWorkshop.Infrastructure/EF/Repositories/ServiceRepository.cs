using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Infrastructure.EF.Context;
using BikeWorkshop.Infrastructure.EF.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BikeWorkshop.Infrastructure.EF.Repositories;

public class ServiceRepository : IServiceRepository
{
	private readonly BikeWorkshopDbContext _context;

	public ServiceRepository(BikeWorkshopDbContext context)
	{
		_context = context;
	}

	public async Task<bool> Add(Service service)
	{
		var entityAddedAction = _context
			.Services
			.AddIfNotExists(service,x => x.Name.ToLower() == service.Name.ToLower());
		if (entityAddedAction is null)
		{
			return false;
		}
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task Delete(Service service)
	{
		_context.Remove(service);
		await _context.SaveChangesAsync();
	}

	public async Task<List<Service>> GetAll()
		=> await _context
		.Services
		.ToListAsync();

	public async Task<Service?> GetById(Guid serviceId)
	{
		return await _context
			.Services
			.Include(x=>x.ServiceToOrders)
			.FirstOrDefaultAsync(x => x.Id == serviceId);
	}

	public async Task<Service?> GetByName(string name)
	{
		return await _context
			.Services
			.FirstOrDefaultAsync(x=>x.Name.ToLower() == name.ToLower());
	}

	public async Task Update(Service service)
	{
		_context.Services.Update(service);
		await _context.SaveChangesAsync();
	}
}
