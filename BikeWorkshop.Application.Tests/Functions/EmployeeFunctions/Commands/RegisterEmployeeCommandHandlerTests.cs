using BikeWorkshop.Application.Functions.EmployeeFunctions.Commands.Register;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Domain.Enums;
using BikeWorkshop.Shared.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace BikeWorkshop.Application.Tests.Functions.EmployeeFunctions.Commands;

public class RegisterEmployeeCommandHandlerTests
{
	private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
	private readonly Mock<IPasswordHasher<Employee>> _passwordHasherMock;
	public RegisterEmployeeCommandHandlerTests()
	{
		_employeeRepositoryMock = new();
		_passwordHasherMock = new();
	}

	[Fact]
	public async Task Handle_Should_RegisterEmployee_When_UniqueEmailAndValidCommand()
	{
		// arrange
		var command = new RegisterEmployeeCommand("Tom", "Res", "tom@gmail.com", "123456789", "123456789", (int)Roles.Worker);
		var handler = new RegisterEmployeeCommandHandler(
			_employeeRepositoryMock.Object,
			_passwordHasherMock.Object);

		_employeeRepositoryMock.Setup(repo => repo.GetByEmail(It.IsAny<string>()))
			.ReturnsAsync((string email) => null);

		// act
		await handler.Handle(command, CancellationToken.None);
		// assert 
		_employeeRepositoryMock.Verify(rep => rep.Register(It.IsAny<Employee>()), Times.Once());
	}
	[Fact]
	public async Task Handle_Should_ThrowException_When_EmailIsNotUnique()
	{
		// arrange
		var command = new RegisterEmployeeCommand("Tom", "Res", "tom@gmail.com", "123456789", "123456789", (int)Roles.Worker);
		var handler = new RegisterEmployeeCommandHandler(
			_employeeRepositoryMock.Object,
			_passwordHasherMock.Object);


		_employeeRepositoryMock.Setup(repo => repo.GetByEmail(It.IsAny<string>()))
			.ReturnsAsync(new Employee());
		// act
		var task = handler.Handle(command, CancellationToken.None);
		// assert 
		await Assert.ThrowsAsync<BadRequestException>(() => task);
	}
}
