using BikeWorkshop.Infrastructure.EF.Context;
using Microsoft.EntityFrameworkCore;

namespace BikeWorkshop.API.Tests.Service;
public static class DatabaseFilters
{
	public static async Task<List<Domain.Entities.Service>> AddServices(this BikeWorkshopDbContext context)
	{
		var serviceList = new List<Domain.Entities.Service>()
		{ 
			new Domain.Entities.Service
			{
				Id = Guid.NewGuid(),
				Name = "First",
			},
			new Domain.Entities.Service
			{
				Id = Guid.NewGuid(),
				Name = "Second",
			},
			new Domain.Entities.Service
			{
				Id = Guid.NewGuid(),
				Name = "Third",
			},
			new Domain.Entities.Service
			{
				Id = Guid.NewGuid(),
				Name = "Fourth",
			},
			new Domain.Entities.Service
			{
				Id = Guid.NewGuid(),
				Name = "Fift",
			},
		};
		await context.Services.AddRangeAsync(serviceList);
		await context.SaveChangesAsync();
		return await context.Services.ToListAsync();
	}
}
