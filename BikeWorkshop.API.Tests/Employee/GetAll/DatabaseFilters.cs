using BikeWorkshop.API.Tests.Settings.DatabaseFilters;
using BikeWorkshop.Domain.Enums;
using BikeWorkshop.Infrastructure.EF.Context;
using Microsoft.EntityFrameworkCore;

namespace BikeWorkshop.API.Tests.Employee.GetAll;
public static class DatabaseFilters
{
	public static async Task<int> AddEmployees(this BikeWorkshopDbContext context)
	{
		List<Domain.Entities.Employee> employees = new List<Domain.Entities.Employee>(20);
		await context.Initialize();

		for (int i = 0; i < 20; i++)
		{
			employees.Add(new Domain.Entities.Employee
			{
				Id = Guid.NewGuid(),
				Email = $"email{i}@email.com",
				FirstName = i.ToString(),
				LastName = i.ToString(),
				PasswordHash = "12343567676redas",
				RoleId = (int)Roles.Worker
			});
		}
		await context.Employees.AddRangeAsync(employees);
		await context.SaveChangesAsync();
		return await context.Employees.CountAsync();
	}
}
