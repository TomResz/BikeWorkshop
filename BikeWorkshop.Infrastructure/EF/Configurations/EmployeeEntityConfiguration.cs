using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeWorkshop.Infrastructure.EF.Configurations;
internal class EmployeeEntityConfiguration()
	: IEntityTypeConfiguration<Employee>
{
	public void Configure(EntityTypeBuilder<Employee> builder)
	{
		builder.Property(x => x.FirstName)
			.HasMaxLength(30);
		builder.Property(x => x.LastName)
			.HasMaxLength(50);
		builder.HasData(GetEmployees());
	}
	private static IEnumerable<Employee> GetEmployees()
	{
		var employees = new List<Employee>()
		{
			new Employee()
			{
				Id = Guid.NewGuid(),
				Email = "admin@admin.com",
				FirstName = "Admin",
				LastName = "Admin",
				RoleId = (int)Roles.Manager,
				PasswordHash = "adminadmin"
			},
			new Employee()
			{
				Id = Guid.NewGuid(),
				Email = "worker@worker.com",
				FirstName = "Worker",
				LastName = "Worker",
				RoleId = (int)Roles.Worker,
				PasswordHash = "workerworker"
			}
		};
		var passwordHasher = new PasswordHasher<Employee>();

		foreach (var employee in employees)
		{
			employee.PasswordHash = passwordHasher.HashPassword(employee, employee.PasswordHash);
		}

		return employees;
	}




}
