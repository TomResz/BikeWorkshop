using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Application.Interfaces.Services;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Shared.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BikeWorkshop.Application.Functions.EmployeeFunctions.Commands.SignIn;

internal sealed class SignInCommandHandler : IRequestHandler<SignInCommand,JwtDto>
{
	private readonly IEmployeeRepository _employeeRepository;
	private readonly IJwtService _jwtService;
	private readonly IPasswordHasher<Employee> _passwordHasher;
	private readonly IRefreshTokenRepository _refreshTokenRepository;
	private readonly IRefreshTokenService _refreshTokenService;
    public SignInCommandHandler(
        IEmployeeRepository employeeRepository,
        IJwtService jwtService,
        IPasswordHasher<Employee> passwordHasher,
        IRefreshTokenRepository refreshTokenRepository,
        IRefreshTokenService refreshTokenService)
    {
        _employeeRepository = employeeRepository;
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
        _refreshTokenRepository = refreshTokenRepository;
        _refreshTokenService = refreshTokenService;
    }

    public async Task<JwtDto> Handle(SignInCommand request, CancellationToken cancellationToken)
	{
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

		var refreshToken = _refreshTokenService.GenerateRefreshToken(employee.Id,DateTime.UtcNow);
		await _refreshTokenRepository.AddAsync(refreshToken);

		return new JwtDto
		{
			AccessToken = _jwtService.GetToken(employee),
			RefreshToken = refreshToken.Token,
		};
	}
}
