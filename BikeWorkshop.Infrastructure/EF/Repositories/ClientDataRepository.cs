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

	public async Task<ClientData?> GetByPhoneNumberOrEmail(string phoneNumber, string? email)
		=> await _context.ClientData
		.FirstOrDefaultAsync(x => x.Email == email 
							|| x.PhoneNumber == phoneNumber);
}
