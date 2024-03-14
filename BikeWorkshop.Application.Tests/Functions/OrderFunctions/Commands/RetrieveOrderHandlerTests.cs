using BikeWorkshop.Application.Functions.OrderFunctions.Command.RetrieveOrder;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Domain.Enums;
using BikeWorkshop.Shared.Exceptions;
using Moq;
using System.Reflection.Emit;

namespace BikeWorkshop.Application.Tests.Functions.OrderFunctions.Commands;
public class RetrieveOrderHandlerTests
{
	private readonly Mock<IOrderRepository> _orderRepository; 
	private readonly Mock<ISummaryRepository> _summaryRepository;
    private readonly RetrieveOrderCommand command = new(Guid.NewGuid());

    public RetrieveOrderHandlerTests()
    {
        _orderRepository = new Mock<IOrderRepository>();
        _summaryRepository = new Mock<ISummaryRepository>();
    }

    [Fact]
    public async Task Handle_UnknownOrder_ThrowsNotFoundException()
    {
        _orderRepository.Setup(x => x.GetById(It.IsAny<Guid>()))
            .ReturnsAsync(() => null);

        var commandHandler = new RetrieveOrderCommandHandler(_orderRepository.Object,_summaryRepository.Object);

        var task = commandHandler.Handle(command, CancellationToken.None);

        await Assert.ThrowsAsync<NotFoundException>(()=> task);
    }

    [Fact]
    public async Task Handle_UnknownSummary_ThrowsNotFoundException()
    {
		_orderRepository.Setup(x => x.GetById(It.IsAny<Guid>()))
	        .ReturnsAsync(() => new());
        _summaryRepository.Setup(x=>x.GetByOrderId(It.IsAny<Guid>()))
            .ReturnsAsync(()=> null);
		var commandHandler = new RetrieveOrderCommandHandler(_orderRepository.Object, _summaryRepository.Object);

		var task = commandHandler.Handle(command, CancellationToken.None);

		await Assert.ThrowsAsync<NotFoundException>(() => task);
	}

    [Theory]
    [InlineData(Status.Retrieved)]
    [InlineData(Status.During)]
    public async Task Handle_NotCompletedOrder_ThrowsBadRequestException(Status status)
    {
		_orderRepository.Setup(x => x.GetById(It.IsAny<Guid>()))
	        .ReturnsAsync(() => new() { OrderStatusId= (int)status});
		_summaryRepository.Setup(x => x.GetByOrderId(It.IsAny<Guid>()))
			.ReturnsAsync(() => new());
		var commandHandler = new RetrieveOrderCommandHandler(_orderRepository.Object, _summaryRepository.Object);

		var task = commandHandler.Handle(command, CancellationToken.None);

        await Assert.ThrowsAsync<BadRequestException>(() => task);  
	}

    [Fact]
    public async Task Handle_ValidCredentials_ShouldRetrievesOrder()
    {
        var order = new Order() { OrderStatusId = (int)Status.Completed };
        var summary = new Summary();
		_orderRepository.Setup(x => x.GetById(It.IsAny<Guid>()))
	        .ReturnsAsync(() => order);
		_summaryRepository.Setup(x => x.GetByOrderId(It.IsAny<Guid>()))
			.ReturnsAsync(() => summary);

		var commandHandler = new RetrieveOrderCommandHandler(_orderRepository.Object, _summaryRepository.Object);
        
        await commandHandler.Handle(command, CancellationToken.None);

        _orderRepository.Verify(x => x.Update(order), Times.Once);
        _summaryRepository.Verify(x => x.Update(summary), Times.Once);
	}
}
