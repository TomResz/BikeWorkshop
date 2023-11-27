using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Interfaces.Repositories;
using MediatR;

namespace BikeWorkshop.Application.Functions.EmployeeFunctions.Queries.GetEmployees;

internal sealed class GetEmployeesQueryHandler
	: IRequestHandler<GetEmployeesQuery, List<EmployeeDto>>
{
	private readonly IEmployeeRepository _employeeRepository;

	public GetEmployeesQueryHandler(IEmployeeRepository employeeRepository)
	{
		_employeeRepository = employeeRepository;
	}

	public async Task<List<EmployeeDto>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
	{
		var employees= await _employeeRepository.GetAll();
		return employees.Select(x => new EmployeeDto
		{
			Id = x.Id,
			FirstName = x.FirstName,
			LastName = x.LastName,
			Email = x.Email,
			RoleName = x.Role.Name
		}).ToList();
	}
}
