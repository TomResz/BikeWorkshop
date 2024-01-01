using BikeWorkshop.Application.Functions.ServiceToOrderFunctions.Commands.Add;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Domain.Enums;
using BikeWorkshop.Shared.Exceptions;
using Moq;

namespace BikeWorkshop.Application.Tests.Functions.ServiceToOrderFunctions.Commands;

public class AddServiceToOrderCommandTests
{
    private readonly Mock<IServiceRepository> _serviceRepositoryMock;
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IServiceToOrderRepository> _serviceToOrderRepositoryMock;

    private AddServiceToOrderCommand _command=new(Guid.NewGuid(), Guid.NewGuid(),2,200);
    public AddServiceToOrderCommandTests()
    {
        _serviceRepositoryMock = new Mock<IServiceRepository>();
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _serviceToOrderRepositoryMock = new Mock<IServiceToOrderRepository>();
    }

    [Fact]
    public async Task Handle_ValidCredentials_ShouldAddsServiceToOrder()
    {
        // arrange
        var command = _command;
        _orderRepositoryMock.Setup(x=>x.GetById(It.IsAny<Guid>()))
            .ReturnsAsync(()=> new Order
            {
                Id = command.ServiceId,
                OrderStatusId = (int)Status.During
            });
        _serviceRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>()))
            .ReturnsAsync(() => new Service()
            {
                Id= Guid.NewGuid(),
                Name = "name"
            });
        var handler = new AddServiceToOrderCommandHandler(_serviceRepositoryMock.Object,
            _orderRepositoryMock.Object, _serviceToOrderRepositoryMock.Object);

        // act
        await handler.Handle(command,default);

        // assert 
        _serviceToOrderRepositoryMock.Verify(x => x.Add(It.IsAny<ServiceToOrder>()),
            Times.Once);
	}

    [Fact]
    public async Task Handle_UnknownOrderId_ShouldThrowsNotFoundException()
    {
		var command = _command;
		_orderRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>()))
			.ReturnsAsync(() => null);
		var handler = new AddServiceToOrderCommandHandler(_serviceRepositoryMock.Object,
	_orderRepositoryMock.Object, _serviceToOrderRepositoryMock.Object);

		// act
		var handleMethod = handler.Handle(command, default);

        // assert
        await Assert.ThrowsAsync<NotFoundException>(() => handleMethod);
	}

    [Theory]
    [InlineData(Status.Completed)]
    [InlineData(Status.Retrieved)]
    public async Task Handle_OrderWithInvalidStatus_ShouldThrowsBadRequestException(Status status)
    {
		var command = _command;
		_orderRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>()))
			.ReturnsAsync(() => new Order
			{
				Id = command.ServiceId,
				OrderStatusId = (int)status
			});
		var handler = new AddServiceToOrderCommandHandler(_serviceRepositoryMock.Object,
_orderRepositoryMock.Object, _serviceToOrderRepositoryMock.Object);

		// act
		var handleMethod = handler.Handle(command, default);

		// assert
		await Assert.ThrowsAsync<BadRequestException>(() => handleMethod);
	}

    [Fact]
    public async Task Handle_InvalidServiceId_ShouldThrowsNotFoundException()
    {
		// arrange
		var command = _command;
		_orderRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>()))
			.ReturnsAsync(() => new Order
			{
				Id = command.ServiceId,
				OrderStatusId = (int)Status.During
			});
		_serviceRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>()))
			.ReturnsAsync(() => null);
		var handler = new AddServiceToOrderCommandHandler(_serviceRepositoryMock.Object,
			_orderRepositoryMock.Object, _serviceToOrderRepositoryMock.Object);

		// act
		var handleMethod =  handler.Handle(command, default);

        // assert
        await Assert.ThrowsAsync<NotFoundException>(() => handleMethod);    
	}
}
