using BikeWorkshop.Application.Functions.SummaryFunctions.Command.Delete;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Domain.Enums;
using BikeWorkshop.Shared.Exceptions;
using Moq;

namespace BikeWorkshop.Application.Tests.Functions.SummaryFunctions.Commands;
public class DeleteSummaryCommandHandlerTests
{
	private readonly Mock<ISummaryRepository> _summaryRepository;
	private readonly Mock<IOrderRepository> _orderRepository;

    private readonly DeleteSummaryCommand _command = new(Guid.NewGuid());
    private  DeleteSummaryCommandHandler _handler;

    public DeleteSummaryCommandHandlerTests()
    {
        _summaryRepository = new Mock<ISummaryRepository>();
        _orderRepository = new Mock<IOrderRepository>();
    }

    [Fact]
    public async Task Handle_UnknownOrder_ThrowsNotFoundException()
    {
        _summaryRepository.Setup(x => x.GetByOrderId(_command.OrderId))
            .ReturnsAsync(() => null);
        _handler = new(_summaryRepository.Object, _orderRepository.Object);

        var task = _handler.Handle(_command, CancellationToken.None);

        await Assert.ThrowsAsync<NotFoundException>(() => task);
    }

    [Fact]
    public async Task Handle_RetrievedOrder_ThrowsBadRequestException()
    {
		_summaryRepository.Setup(x => x.GetByOrderId(_command.OrderId))
			.ReturnsAsync(() => new() 
            { 
                Order =new() { OrderStatusId = (int)Status.Retrieved}
            });
		_handler = new(_summaryRepository.Object, _orderRepository.Object);

		var task = _handler.Handle(_command, CancellationToken.None);

		await Assert.ThrowsAsync<BadRequestException>(() => task);
	}

    [Fact]
    public async Task Handle_ValidData_DeletesSummary()
    {
		_summaryRepository.Setup(x => x.GetByOrderId(_command.OrderId))
			.ReturnsAsync(() => new()
			{
				Order = new() { OrderStatusId = (int)Status.Completed }
			});
		_handler = new(_summaryRepository.Object, _orderRepository.Object);

		await _handler.Handle(_command, CancellationToken.None);

        _summaryRepository.Verify(x=> x.Delete(It.IsAny<Summary>()),Times.Once);
        _orderRepository.Verify(x=> x.Update(It.IsAny<Order>()),Times.Once);
	}

}
