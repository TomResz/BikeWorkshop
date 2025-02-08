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

	public async Task<List<Employee>> GetAll()
		=> await _context
		.Employees
		.Include(x=>x.Role)
		.AsNoTracking()
		.ToListAsync();

	public async Task<Employee?> GetByEmail(string email)
	{
		return await _context
			.Employees
			.Include(x=>x.Role)
			.FirstOrDefaultAsync(x => x.Email == email.ToLower());
	}

	public async Task<Employee?> GetById(Guid id)
	{
		return await _context.Employees
			.Include(x => x.Role)
			.FirstOrDefaultAsync(x => x.Id == id);
	}

	public async Task<Employee?> GetByIdAndRefreshTokenAsync(Guid id, string refreshToken)
		=> await _context.Employees
		.Include(x=> x.Role)
		.Include(x => x.RefreshTokens)
		.Where(x => x.Id == id &&
			x.RefreshTokens.Any(x => x.Token == refreshToken))
		.FirstOrDefaultAsync();

    public async Task Register(Employee employee)
	{
		await _context 
			.Employees 
			.AddAsync(employee);
		await _context.SaveChangesAsync();
	}

	public async Task Update(Employee employee)
	{
		_context.Employees.Update(employee);
		await _context.SaveChangesAsync();
	}
}
