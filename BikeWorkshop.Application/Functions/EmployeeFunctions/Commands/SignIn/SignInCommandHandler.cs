using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Application.Interfaces.Services;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Shared.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BikeWorkshop.Application.Functions.EmployeeFunctions.Commands.SignIn;

internal sealed class SignInCommandHandler : IRequestHandler<SignInCommand,SignInResponse>
{
	private readonly IEmployeeRepository _employeeRepository;
	private readonly IJwtService _jwtService;
	private readonly IValidator<SignInCommand> _validator;
	private readonly IPasswordHasher<Employee> _passwordHasher;
	public SignInCommandHandler(
		IEmployeeRepository employeeRepository,
		IJwtService jwtService,
		IValidator<SignInCommand> validator,
		IPasswordHasher<Employee> passwordHasher)
	{
		_employeeRepository = employeeRepository;
		_jwtService = jwtService;
		_validator = validator;
		_passwordHasher = passwordHasher;
	}

	public async Task<SignInResponse> Handle(SignInCommand request, CancellationToken cancellationToken)
	{
		var result = await _validator.ValidateAsync(request, cancellationToken);
		if(!result.IsValid)
		{
			throw new BadRequestException(result.ToString());
		}
		var employee = await _employeeRepository.GetByEmail(request.Email);
        if (employee is null)
        {
			throw new NotFoundException("Incorrect email or password!");
        }
		var passwordResponse = _passwordHasher.VerifyHashedPassword(employee, employee.PasswordHash, request.Password);

		if(passwordResponse == PasswordVerificationResult.Failed) 
		{
			throw new NotFoundException("Incorrect email or password!");
		}

		return new SignInResponse(_jwtService.GetToken(employee));
	}
}
