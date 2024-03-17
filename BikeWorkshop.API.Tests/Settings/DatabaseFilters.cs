using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Domain.Enums;
using BikeWorkshop.Infrastructure.EF.Context;
using Microsoft.AspNetCore.Identity;

namespace BikeWorkshop.API.Tests.Settings;
public static partial class DatabaseFilters
{
	public static async Task Initialize(this BikeWorkshopDbContext context)
	{
		PasswordHasher<Domain.Entities.Employee> passwordHasher = new PasswordHasher<Domain.Entities.Employee>();

		var roles = new List<Role>
		{
			new(){Id = (int)Roles.Worker, Name = Roles.Worker.ToString()},
			new(){Id = (int)Roles.Manager, Name = Roles.Manager.ToString()},
		};

		var manager = new Domain.Entities.Employee
		{
			Email = "manager@manager.com",
			FirstName = "Manager",
			Id = Constants.ManagerId,
			LastName = "Manager",
			PasswordHash = Constants.Password,
			RoleId = (int)Roles.Manager
		};
		var emploeyee = new Domain.Entities.Employee
		{
			Email = "worker@worker.com",
			FirstName = "Worker",
			Id = Constants.EmployeeId,
			LastName = "Worker",
			PasswordHash = Constants.Password,
			RoleId = (int)Roles.Worker
		};
		manager.PasswordHash = passwordHasher.HashPassword(manager,manager.PasswordHash);
		emploeyee.PasswordHash = passwordHasher.HashPassword(emploeyee, emploeyee.PasswordHash);

		if (!context.Roles.Any())
		{
			await context.Roles.AddRangeAsync(roles);
			await context.SaveChangesAsync();
		}

		if (!context.Employees.Any())
		{
			await context.Employees.AddAsync(manager);
			await context.Employees.AddAsync(emploeyee);
			await context.SaveChangesAsync();
		}
	}
}
