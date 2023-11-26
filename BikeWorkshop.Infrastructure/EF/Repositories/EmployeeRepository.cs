using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Infrastructure.EF.Context;
using Microsoft.EntityFrameworkCore;

namespace BikeWorkshop.Infrastructure.EF.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
	private readonly BikeWorkshopDbContext _context;

	public EmployeeRepository(BikeWorkshopDbContext context)
	{
		_context = context;
	}

	public async Task<Employee?> GetByEmail(string email)
	{
		return await _context
			.Employees
			.Include(x=>x.Role)
			.FirstOrDefaultAsync(x => x.Email == email.ToLower());
	}

	public async Task Register(Employee employee)
	{
		await _context 
			.Employees 
			.AddAsync(employee);
		await _context.SaveChangesAsync();
	}

	public Task Update(Employee employee)
	{
		return Task.CompletedTask;
	}
}
