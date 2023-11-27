using BikeWorkshop.Application.Functions.EmployeeFunctions.Commands.UpdatePassword;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Application.Interfaces.Services;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Shared.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace BikeWorkshop.Application.Tests.Functions.EmployeeFunctions.Commands;

public class UpdatePasswordCommandHandlerTests
{
	private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
	private readonly Mock<IPasswordHasher<Employee>> _passwordHasherMock;
	private readonly Mock<IValidator<UpdatePasswordCommand>> _validatorMock;
	private readonly Mock<IEmployeeSessionContext> _employeeSessionContextMock;

	public UpdatePasswordCommandHandlerTests()
	{
		_employeeRepositoryMock = new Mock<IEmployeeRepository>();
		_employeeSessionContextMock = new Mock<IEmployeeSessionContext>();
		_validatorMock = new Mock<IValidator<UpdatePasswordCommand>>();
		_passwordHasherMock = new Mock<IPasswordHasher<Employee>>();
	}

	[Fact]
	public async Task Handle_WithValidCredential_ShouldUpdatePassword()
	{
		// Arrange
		var employeeId = Guid.NewGuid();
		var command = new UpdatePasswordCommand("12345678", "12345678");

		_employeeSessionContextMock.Setup(e => e.GetEmployeeId()).Returns(employeeId);

		var employee = new Employee { Id = employeeId };
		_employeeRepositoryMock.Setup(e => e.GetById(It.IsAny<Guid>())).ReturnsAsync(employee);
		_passwordHasherMock.Setup(p => p.HashPassword(It.IsAny<Employee>(), It.IsAny<string>()))
						  .Returns("hashedPassword");
		_validatorMock.Setup(e => e.ValidateAsync(It.IsAny<UpdatePasswordCommand>(), CancellationToken.None))
			.ReturnsAsync(new FluentValidation.Results.ValidationResult());
		var handler = new UpdatePasswordCommandHandler(
			_employeeRepositoryMock.Object,
			_employeeSessionContextMock.Object,
			_validatorMock.Object,
			_passwordHasherMock.Object);

		// Act
		await handler.Handle(command, CancellationToken.None);

		// Assert
		_validatorMock.Verify(v => v.ValidateAsync(command, CancellationToken.None), Times.Once);
		_employeeSessionContextMock.Verify(e => e.GetEmployeeId(), Times.Once);
		_employeeRepositoryMock.Verify(r => r.GetById(employeeId), Times.Once);
		_passwordHasherMock.Verify(p => p.HashPassword(employee, "12345678"), Times.Once);
		_employeeRepositoryMock.Verify(r => r.Update(It.Is<Employee>(e => e.PasswordHash == "hashedPassword")), Times.Once);
	}

	[Fact]
	public async Task Handle_WithUnauthorizedEmployee_ThrowsUnauthorizedAccessException()
	{
		// Arrange
		var employeeId = Guid.NewGuid();
		var command = new UpdatePasswordCommand("12345678", "12345678");
		_employeeSessionContextMock.Setup(e => e.GetEmployeeId()).Returns(() => null);
		_validatorMock.Setup(e => e.ValidateAsync(It.IsAny<UpdatePasswordCommand>(), CancellationToken.None))
			.ReturnsAsync(new FluentValidation.Results.ValidationResult());
		var handler = new UpdatePasswordCommandHandler(
			_employeeRepositoryMock.Object,
			_employeeSessionContextMock.Object,
			_validatorMock.Object,
			_passwordHasherMock.Object);
		// Act
		var resultTask = handler.Handle(command, CancellationToken.None);
		// Assert
		await Assert.ThrowsAsync<UnauthorizedException>(() => resultTask);
	}

	[Fact]
	public async Task Handle_WithInvalidCredentials_ThrowsBadRequestException()
	{
		// arrange
		var command = new UpdatePasswordCommand("12345678", "");
		_validatorMock.Setup(s => s.ValidateAsync(command, CancellationToken.None))
			.ReturnsAsync(new FluentValidation.Results.ValidationResult(new[]
			{
				new ValidationFailure("ConfirmedPassword","Password don't match!")
			}));

		var handler = new UpdatePasswordCommandHandler(
			_employeeRepositoryMock.Object,
			_employeeSessionContextMock.Object,
			_validatorMock.Object,
			_passwordHasherMock.Object);
		// act
		var resultTask = handler.Handle(command, default);
		// arrange
		await Assert.ThrowsAsync<BadRequestException>(() => resultTask);
	}
}
