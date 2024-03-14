using BikeWorkshop.Application.Functions.SummaryFunctions.Command.CreateSummaryForOrder;
using BikeWorkshop.Application.Interfaces.Email;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Application.Interfaces.Services;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Domain.Enums;
using BikeWorkshop.Shared.Exceptions;
using Moq;

namespace BikeWorkshop.Application.Tests.Functions.SummaryFunctions.Commands;
public class CreateSummaryForOrderCommandHandlerTests
{
	private readonly Mock<IOrderRepository> _orderRepository;
	private readonly Mock<ISummaryRepository> _summaryRepository;
	private readonly Mock<IServiceToOrderRepository> _serviceToOrderRepository;
	private readonly Mock<IClientDataRepository> _clientDataRepository;
	private readonly Mock<ICustomEmailSender> _customEmailSender;
	private readonly Mock<ISummaryEmailContent> _summaryEmailContent;
	private readonly CreateSummaryForOrderCommand _command = new(Guid.NewGuid());
	private CreateSummaryForOrderCommandHandler _handler;

	private readonly List<ServiceToOrder> _serviceToOrders = new()
	{
		new(){Count = 1, Price = 100m},
		new(){Count = 2, Price = 50m},
		new(){Count = 3, Price = 70m},
	};

	public CreateSummaryForOrderCommandHandlerTests()
	{
		_orderRepository = new Mock<IOrderRepository>();
		_summaryEmailContent = new Mock<ISummaryEmailContent>();
		_serviceToOrderRepository = new Mock<IServiceToOrderRepository>();
		_summaryRepository = new Mock<ISummaryRepository>();
		_clientDataRepository = new Mock<IClientDataRepository>();
		_customEmailSender = new Mock<ICustomEmailSender>();
	}

	[Fact]
	public async Task Handle_UnknownOrderId_ThrowsNotFoundException()
	{
		_orderRepository.Setup(x => x.GetById(_command.OrderId))
			.ReturnsAsync(() => null);

		_handler = new(_orderRepository.Object,
			_summaryRepository.Object, _serviceToOrderRepository.Object,
			_clientDataRepository.Object, _customEmailSender.Object, _summaryEmailContent.Object);

		var task = _handler.Handle(_command, CancellationToken.None);

		await Assert.ThrowsAsync<NotFoundException>(() => task);
	}

	[Theory]
	[InlineData(Status.Retrieved)]
	[InlineData(Status.Completed)]
	public async Task Handle_NotCorrectOrderStatus_ThrowsBadRequestException(Status status)
	{
		_orderRepository.Setup(x => x.GetById(_command.OrderId))
			.ReturnsAsync(() => new() { OrderStatusId = (int)status });


		_handler = new(_orderRepository.Object,
	_summaryRepository.Object, _serviceToOrderRepository.Object,
	_clientDataRepository.Object, _customEmailSender.Object, _summaryEmailContent.Object);

		var task = _handler.Handle(_command, CancellationToken.None);

		await Assert.ThrowsAsync<BadRequestException>(() => task);
	}

	[Fact]
	public async Task Handle_ValidDataWithEmptyEmail_ShouldCreateSummary()
	{
		_orderRepository.Setup(x => x.GetById(_command.OrderId))
			.ReturnsAsync(() => new() { OrderStatusId = (int)Status.During , Summary = new()});
		_clientDataRepository.Setup(x => x.GetEmailByOrderId(_command.OrderId))
			.ReturnsAsync(() => null);
		_serviceToOrderRepository.Setup(x => x.GetByOrderId(_command.OrderId))
			.ReturnsAsync(() => _serviceToOrders);

		_handler = new(_orderRepository.Object,
_summaryRepository.Object, _serviceToOrderRepository.Object,
_clientDataRepository.Object, _customEmailSender.Object, _summaryEmailContent.Object);

		await _handler.Handle(_command, CancellationToken.None);

		_orderRepository.Verify(x => x.Update(It.IsAny<Order>()), Times.Once);
		_summaryRepository.Verify(x => x.Add(It.IsAny<Summary>()), Times.Once);
		_customEmailSender.Verify(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
	}

	[Fact]
	public async Task Handle_ValidDataWithCustomerEmail_ShouldCreateSummary()
	{
		_orderRepository.Setup(x => x.GetById(_command.OrderId))
			.ReturnsAsync(() => new() { OrderStatusId = (int)Status.During, Summary = new() });
		_clientDataRepository.Setup(x => x.GetEmailByOrderId(_command.OrderId))
			.ReturnsAsync(() => null);
		_serviceToOrderRepository.Setup(x => x.GetByOrderId(_command.OrderId))
			.ReturnsAsync(() => _serviceToOrders);
		_clientDataRepository.Setup(x => x.GetEmailByOrderId(_command.OrderId))
			.ReturnsAsync(() =>  "email@email.com");



		_handler = new(_orderRepository.Object,
_summaryRepository.Object, _serviceToOrderRepository.Object,
_clientDataRepository.Object, _customEmailSender.Object, _summaryEmailContent.Object);

		await _handler.Handle(_command, CancellationToken.None);

		_orderRepository.Verify(x => x.Update(It.IsAny<Order>()), Times.Once);
		_summaryRepository.Verify(x => x.Add(It.IsAny<Summary>()), Times.Once);
		_customEmailSender.Verify(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
	}

}
