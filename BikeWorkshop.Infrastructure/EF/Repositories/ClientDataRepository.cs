using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Infrastructure.EF.Context;
using Microsoft.EntityFrameworkCore;

namespace BikeWorkshop.Infrastructure.EF.Repositories;

public class ClientDataRepository : IClientDataRepository
{
	private readonly BikeWorkshopDbContext _context;

	public ClientDataRepository(BikeWorkshopDbContext context)
	{
		_context = context;
	}

	public async Task Add(ClientData data)
	{
		await _context
			.ClientData
			.AddAsync(data);
		await _context.SaveChangesAsync();
	}

	public async Task<ClientData?> GeByOrderId(Guid orderId)
		=> await _context.ClientData
		.Include(x=>x.Orders)
		.Where(x=> x.Orders.Any(x=> x.Id == orderId))
		.FirstOrDefaultAsync();

	public async Task<ClientData?> GetByPhoneNumberOrEmail(string phoneNumber, string? email)
		=> await _context.ClientData
		.FirstOrDefaultAsync(x => x.Email == email 
							|| x.PhoneNumber == phoneNumber);

	public async Task<ClientData?> GetByShortId(string shortId)
		=> await _context.ClientData
		.Include(x => x.Orders)
		.Where(x => x.Orders.Any(x => x.ShortUniqueId == shortId))
		.FirstOrDefaultAsync();

	public async Task<string?> GetEmailByOrderId(Guid orderId)
		=> await _context
		.Orders
		.Include(x => x.ClientData)
		.AsNoTracking()
		.Where(x => x.Id == orderId)
		.Select(x => x.ClientData.Email)
		.FirstOrDefaultAsync();
}
