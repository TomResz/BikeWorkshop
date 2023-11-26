using BikeWorkshop.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BikeWorkshop.Infrastructure.Services;

public class EmployeeSessionContext : IEmployeeSessionContext
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public EmployeeSessionContext(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}
	private ClaimsPrincipal? Employee => _httpContextAccessor.HttpContext?.User;
	public Guid? GetEmployeeId()
	{
		if (Employee is null)
			return null;
		var nameIdentifierClaim = Employee.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
		if (nameIdentifierClaim is null || !Guid.TryParse(nameIdentifierClaim.Value, out var employeeId))
			return null;
		return employeeId;
	}
}
