using BikeWorkshop.Application.Functions.EmployeeFunctions.Commands.UpdatePassword;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Application.Interfaces.Services;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace BikeWorkshop.Application.Tests.Functions.EmployeeFunctions.Commands;

public class UpdatePasswordCommandHandlerTests
{
	private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
	private readonly Mock<IPasswordHasher<Employee>> _passwordHasherMock;
	private readonly Mock<IEmployeeSessionContext> _employeeSessionContextMock;

	public UpdatePasswordCommandHandlerTests()
	{
		_employeeRepositoryMock = new Mock<IEmployeeRepository>();
		_employeeSessionContextMock = new Mock<IEmployeeSessionContext>();
		_passwordHasherMock = new Mock<IPasswordHasher<Employee>>();
	}

	[Fact]
	public async Task Handle_WithUnauthorizedEmployee_ThrowsUnauthorizedAccessException()
	{
		// Arrange
		var employeeId = Guid.NewGuid();
		var command = new UpdatePasswordCommand("12345678", "12345678");
		_employeeSessionContextMock.Setup(e => e.GetEmployeeId()).Returns(() => null);
		var handler = new UpdatePasswordCommandHandler(
			_employeeRepositoryMock.Object,
			_employeeSessionContextMock.Object,
			_passwordHasherMock.Object);
		// Act
		var resultTask = handler.Handle(command, CancellationToken.None);
		// Assert
		await Assert.ThrowsAsync<UnauthorizedException>(() => resultTask);
	}

}
