﻿using BikeWorkshop.Application.Functions.EmployeeFunctions.Commands.SignIn;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Application.Interfaces.Services;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Shared.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace BikeWorkshop.Application.Tests.Functions.EmployeeFunctions.Commands;

public class SignInCommandHandlerTests
{
	private readonly Mock<IEmployeeRepository> _repositoryMock;
	private readonly Mock<IJwtService> _jwtServiceMock;
	private readonly Mock<IValidator<SignInCommand>> _validatorMock;
	private readonly Mock<IPasswordHasher<Employee>> _passwordHasherMock;
    public SignInCommandHandlerTests()
    {
        _repositoryMock = new Mock<IEmployeeRepository>();
		_jwtServiceMock = new Mock<IJwtService>();
		_validatorMock = new Mock<IValidator<SignInCommand>>();
		_passwordHasherMock = new Mock<IPasswordHasher<Employee>>();
    }

	[Fact]
	public async Task Handle_WithValidCredentials_ReturnSignInResponse()
	{
		// Arrange
		var employee = new Employee
		{
			Email = "test@example.com",
			FirstName = "Test",
			LastName = "Test",
			Id = Guid.NewGuid(),
					
		};
		var hashed = _passwordHasherMock.Object.HashPassword(employee,"password");
		employee.PasswordHash = hashed;


		_repositoryMock.Setup(repo => repo.GetByEmail(It.IsAny<string>()))
			.ReturnsAsync(employee);

		_jwtServiceMock.Setup(service => service.GetToken(It.IsAny<Employee>()))
			.Returns("mocked-token");

		_passwordHasherMock.Setup(hasher =>
				hasher.VerifyHashedPassword(It.IsAny<Employee>(), It.IsAny<string>(), It.IsAny<string>()))
			.Returns(PasswordVerificationResult.Success);

		var signInCommand = new SignInCommand("test@example.com", "password");

		_validatorMock.Setup(validator => validator.ValidateAsync(It.IsAny<SignInCommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new FluentValidation.Results.ValidationResult());

		var signInCommandHandler = new SignInCommandHandler(
			_repositoryMock.Object,
			_jwtServiceMock.Object,
			_validatorMock.Object,
			_passwordHasherMock.Object
		);

		// Act
		var result = await signInCommandHandler.Handle(signInCommand, CancellationToken.None);

		// Assert
		Assert.NotNull(result);
		Assert.Equal("mocked-token", result.Token);
	}
	[Fact]
	public async Task Handle_WithValidationErrors_ThrowsBadRequestException()
	{
		// Arrange
		var signInCommand = new SignInCommand("", "");

		_validatorMock.Setup(validator => validator.ValidateAsync(It.IsAny<SignInCommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new FluentValidation.Results.ValidationResult(new[]{
			new ValidationFailure("Email", "Email is required."),
			new ValidationFailure("Password", "Password is required.")
			}));
		var signInCommandHandler = new SignInCommandHandler(
			_repositoryMock.Object,
			_jwtServiceMock.Object,
			_validatorMock.Object,
			_passwordHasherMock.Object
			);
		// Act
		var resultTask =  signInCommandHandler.Handle(signInCommand, CancellationToken.None);

		// Assert
		await Assert.ThrowsAsync<BadRequestException>(()=> resultTask);
	}


	[Fact]
	public async Task Handle_WithInvalidPassword_ThrowsNotFoundException()
	{
		// Arranged
		var employee = new Employee
		{
			Email = "test@example.com",
			FirstName = "Test",
			LastName = "Test",
			Id = Guid.NewGuid(),
		};

		var hashed = _passwordHasherMock.Object.HashPassword(employee, "differentpassword");
		employee.PasswordHash = hashed;

		_validatorMock.Setup(repo => repo.ValidateAsync(It.IsAny<SignInCommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new ValidationResult());
		
		_repositoryMock.Setup(repo => repo.GetByEmail(It.IsAny<string>()))
			.ReturnsAsync(employee);

		var signInCommand = new SignInCommand("test@example.com", "invalidpassword");
		var signInCommandHandler = new SignInCommandHandler(
				_repositoryMock.Object,
				_jwtServiceMock.Object,
				_validatorMock.Object,
				_passwordHasherMock.Object
				);
		// Act
		var resultTask =  signInCommandHandler.Handle(signInCommand, CancellationToken.None);

		// Assert
		await Assert.ThrowsAsync<NotFoundException>(()=>resultTask);
	}

	[Fact]
	public async Task Handle_WithUknownEmail_ThrowsNotFoundException()
	{
		// Arranged
		var signInCommand = new SignInCommand("test@example.com", "test");

		_validatorMock.Setup(repo => repo.ValidateAsync(It.IsAny<SignInCommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new ValidationResult());
		_repositoryMock.Setup(repo => repo.GetByEmail(It.IsAny<string>()))
			.ReturnsAsync((string email) => null);
		var handler = new SignInCommandHandler(
				_repositoryMock.Object,
				_jwtServiceMock.Object,
				_validatorMock.Object,
				_passwordHasherMock.Object);
		// Act
		var resultTask = handler.Handle(signInCommand, default);

		// Assert
		await Assert.ThrowsAsync<NotFoundException>(() => resultTask);
	}
}
