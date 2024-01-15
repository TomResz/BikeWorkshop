using BikeWorkshop.Application.Functions.ServiceToOrderFunctions.Commands.Delete;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Shared.Exceptions;
using Moq;

namespace BikeWorkshop.Application.Tests.Functions.ServiceToOrderFunctions.Commands;

public class DeleteServiceToOrderCommandHandlerTests
{
	private readonly Mock<IServiceToOrderRepository> _serviceToOrderRepository;

	public DeleteServiceToOrderCommandHandlerTests()
	{
		_serviceToOrderRepository = new();
	}

	[Fact]
	public async Task Handle_ValidCommand_ShouldDelete()
	{
		var command = new DeleteServiceToOrderCommand(Guid.NewGuid());
		_serviceToOrderRepository.Setup(x => x.GetById(command.ServiceToOrderId))
			.ReturnsAsync(() => new ServiceToOrder
			{
				Id = command.ServiceToOrderId,
				Price = 100m,
				Count = 1,
			});
		var commandHandler = new DeleteServiceToOrderCommandHandler(_serviceToOrderRepository.Object);

		await commandHandler.Handle(command, default);

		_serviceToOrderRepository.Verify(x=>x.Delete(It.IsAny<ServiceToOrder>()),Times.Once);
	}


	[Fact]
	public async Task Handle_ValidCommand_ShouldThrowsNotFoundException()
	{
		var command = new DeleteServiceToOrderCommand(Guid.NewGuid());
		_serviceToOrderRepository.Setup(x => x.GetById(command.ServiceToOrderId))
			.ReturnsAsync(() => null);
		var commandHandler = new DeleteServiceToOrderCommandHandler(_serviceToOrderRepository.Object);

		var handleMethod =  commandHandler.Handle(command, default);
		await Assert.ThrowsAsync<NotFoundException>(()=> handleMethod);	
		_serviceToOrderRepository.Verify(x => x.Delete(It.IsAny<ServiceToOrder>()), Times.Never);
	}
}
