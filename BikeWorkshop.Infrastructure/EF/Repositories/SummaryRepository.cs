using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Infrastructure.EF.Context;
using Microsoft.EntityFrameworkCore;

namespace BikeWorkshop.Infrastructure.EF.Repositories;
internal class SummaryRepository : ISummaryRepository
{
	private readonly BikeWorkshopDbContext _context;

	public SummaryRepository(BikeWorkshopDbContext context)
	{
		_context = context;
	}

	public async Task Add(Summary summary)
	{
		await _context.Summaries.AddAsync(summary);	
		await _context.SaveChangesAsync();
	}

	public async Task<Summary?> GetByOrderId(Guid orderId)
		=> await _context
		.Summaries
		.AsNoTracking()
		.FirstOrDefaultAsync(x=>x.OrderId == orderId);
}
