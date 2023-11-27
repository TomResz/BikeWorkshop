using BikeWorkshop.Application.Functions.DTO;
using MediatR;

namespace BikeWorkshop.Application.Functions.EmployeeFunctions.Queries.GetEmployees;

public sealed record GetEmployeesQuery() : IRequest<List<EmployeeDto>>;
