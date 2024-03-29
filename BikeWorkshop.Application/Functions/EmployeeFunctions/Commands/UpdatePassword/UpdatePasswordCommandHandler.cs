﻿using BikeWorkshop.Application.Fluent_Validation_Extensions;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Application.Interfaces.Services;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Shared.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BikeWorkshop.Application.Functions.EmployeeFunctions.Commands.UpdatePassword;

internal class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand>
{
	private readonly IEmployeeRepository _employeeRepository;
	private readonly IEmployeeSessionContext _sessionContext;
	private readonly IPasswordHasher<Employee> _passwordHasher;
	public UpdatePasswordCommandHandler(
		IEmployeeRepository employeeRepository,
		IEmployeeSessionContext sessionContext,
		IPasswordHasher<Employee> passwordHasher)
	{
		_employeeRepository = employeeRepository;
		_sessionContext = sessionContext;
		_passwordHasher = passwordHasher;
	}

	public async Task Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
	{
		var employeeId = _sessionContext.GetEmployeeId()
				?? throw new UnauthorizedException("Unauthorized access!");

		var employee = await _employeeRepository.GetById(employeeId);
		if (employee is null)
		{
			throw new NotFoundException("Employee not found!");
		}
		var newPassword = _passwordHasher.HashPassword(employee, request.Password);
		employee.PasswordHash = newPassword;
		await _employeeRepository.Update(employee);
    }
}
