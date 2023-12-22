using BikeWorkshop.Application.Functions.OrderFunctions.Command.CreateOrder;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Application.Interfaces.Services;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Shared.Exceptions;
using FluentValidation;
using Moq;

namespace BikeWorkshop.Application.Tests.Functions.OrderFunctions.Commands;

public class CreateOrderCommandHandlerTests
{
	private readonly Mock<IOrderRepository> _orderRepositoryMock;
	private readonly Mock<IEmployeeSessionContext> _employeeSessionContextMock;
	private readonly Mock<IValidator<CreateOrderCommand>> _validatorMock;
	private readonly Mock<IShortIdService> _shortIdServiceMock;

	public CreateOrderCommandHandlerTests()
	{
		_orderRepositoryMock = new Mock<IOrderRepository>();
		_employeeSessionContextMock = new Mock<IEmployeeSessionContext>();
		_validatorMock = new Mock<IValidator<CreateOrderCommand>>();
		_shortIdServiceMock = new Mock<IShortIdService>();

	}

	[Fact]
	public async Task Handle_UnauthorizedEmployee_ThrowsUnauthorizedException()
	{
		var command = new CreateOrderCommand("Example desc", Guid.NewGuid());
		_employeeSessionContextMock.Setup(x => x.GetEmployeeId())
			.Returns(() => null);
		var handler = new CreateOrderCommandHandler(_orderRepositoryMock.Object,
			_employeeSessionContextMock.Object,
			_validatorMock.Object,
			_shortIdServiceMock.Object);

		var task = handler.Handle(command, CancellationToken.None);

		await Assert.ThrowsAsync<UnauthorizedException>(() => task);
	}


	[Fact]
	public async Task Handle_CorrectData_ShouldCreateNewOrder()
	{
		var command = new CreateOrderCommand("New desc", Guid.NewGuid());
		_employeeSessionContextMock.Setup(x => x.GetEmployeeId())
			.Returns(() => Guid.NewGuid());
		_validatorMock.Setup(x => x.ValidateAsync(command, CancellationToken.None))
			.ReturnsAsync(new FluentValidation.Results.ValidationResult());
		var handler = new CreateOrderCommandHandler(_orderRepositoryMock.Object,
		_employeeSessionContextMock.Object,
		_validatorMock.Object,
		_shortIdServiceMock.Object);


		await handler.Handle(command, CancellationToken.None);


		_orderRepositoryMock.Verify(x => x.Add(It.IsAny<Order>()), Times.Once);

	}
}
