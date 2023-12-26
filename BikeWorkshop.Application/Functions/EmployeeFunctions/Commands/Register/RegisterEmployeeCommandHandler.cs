using BikeWorkshop.Application.Fluent_Validation_Extensions;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Shared.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BikeWorkshop.Application.Functions.EmployeeFunctions.Commands.Register;

internal class RegisterEmployeeCommandHandler : IRequestHandler<RegisterEmployeeCommand>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IPasswordHasher<Employee> _passwordHasher;
	public RegisterEmployeeCommandHandler(
		IEmployeeRepository employeeRepository,
		IPasswordHasher<Employee> passwordHasher)
	{
		_employeeRepository = employeeRepository;
		_passwordHasher = passwordHasher;
	}

	public async Task Handle(RegisterEmployeeCommand request, CancellationToken cancellationToken)
    {
        var gettedByEmail = await _employeeRepository.GetByEmail(request.Email);
        if (gettedByEmail is not null)
        {
            throw new BadRequestException("Email already exists!");
        }
        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            Email = request.Email.ToLower(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            RoleId = request.RoleId,
        };
        var passwHash = _passwordHasher.HashPassword(employee, request.Password);
        employee.PasswordHash = passwHash;
        await _employeeRepository.Register(employee);
    }
}
