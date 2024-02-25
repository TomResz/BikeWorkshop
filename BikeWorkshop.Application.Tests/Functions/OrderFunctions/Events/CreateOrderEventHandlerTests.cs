using BikeWorkshop.Application.Functions.ClientDataFunctions.Commands.CreateClientData;
using BikeWorkshop.Application.Functions.OrderFunctions.Command.CreateOrder;
using BikeWorkshop.Application.Functions.OrderFunctions.Events.CreateOrder;
using BikeWorkshop.Application.Interfaces.Email;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Application.Interfaces.Services;
using BikeWorkshop.Domain.Entities;
using MediatR;
using Moq;

namespace BikeWorkshop.Application.Tests.Functions.OrderFunctions.Events;

public class CreateOrderEventHandlerTests
{
	private readonly Mock<IMediator> _mockMediator;
	private readonly Mock<IClientDataRepository> _mockClientDataRepository;
    private readonly Mock<ICustomEmailSender> _mockCustomEmailSender;
	private readonly Mock<ICreateOrderEmailContent> _mockCreateOrderEmailContent;
	private readonly Mock<IOrderTrackingURL> _mockOrderTrackingUrl;
    public CreateOrderEventHandlerTests()
    {
        _mockClientDataRepository = new();
        _mockMediator = new();
        _mockCustomEmailSender = new Mock<ICustomEmailSender>();
		_mockCreateOrderEmailContent = new();
		_mockOrderTrackingUrl = new();
    }


    [Fact]
    public async Task Handle_WithUnknownCustomer_Should_CreateClientData()
    {
		// arrange
		var expectedOrderId = Guid.NewGuid();
		var expectedShortId = "ABC123";
		var expectedAddedDate = DateTime.Now;


		var @event = new CreateOrderEvent("tomasz@gmail.com", "+48999777555", "Example description.");
        _mockClientDataRepository.Setup(rep => rep.GetByPhoneNumberOrEmail(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((string email, string phoneNumber) => null);


		_mockMediator.Setup(x => x.Send(It.IsAny<CreateOrderCommand>(), CancellationToken.None))
	        .ReturnsAsync(() => new(expectedOrderId, expectedShortId, expectedAddedDate));
		_mockMediator.Setup(x => x.Send(It.IsAny<CreateClientDataCommand>(), CancellationToken.None))
			.ReturnsAsync(() => new()
			{
				Email = @event.Email,
				Id = Guid.NewGuid(),
				PhoneNumber = @event.PhoneNumber
			});
		var handler = new CreateOrderEventHandler(_mockMediator.Object, _mockClientDataRepository.Object
			, _mockCustomEmailSender.Object, _mockCreateOrderEmailContent.Object, _mockOrderTrackingUrl.Object);
		// act
		await handler.Handle(@event, CancellationToken.None);

        // assert
        _mockMediator.Verify(x=>x.Send(It.Is<CreateClientDataCommand>(
            x=>x.PhoneNumber == @event.PhoneNumber && x.Email == @event.Email), CancellationToken.None),Times.Once);
        _mockMediator.Verify(x => x.Send(It.Is<CreateOrderCommand>(
            x => x.Description == @event.Description), CancellationToken.None),Times.Once);
        _mockCustomEmailSender.Verify(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())
        , Times.Once);
    }
	[Fact]
	public async Task Handle_WithExistingCustomer_Should_OnlyCreateOrder_WithoutCreatingNewClientData()
	{
		// arrange
        var clientData = new ClientData()
        {
            Id = Guid.NewGuid(),
            Email = "tomasz@gmail.com",
            PhoneNumber = "48999777555"
		};

		var expectedOrderId = Guid.NewGuid();
		var expectedShortId = "ABC123";
		var expectedAddedDate = DateTime.Now;

		var @event = new CreateOrderEvent(clientData.Email, clientData.PhoneNumber, "Example description.");
		_mockClientDataRepository.Setup(rep => rep.GetByPhoneNumberOrEmail(It.IsAny<string>(), It.IsAny<string>()))
			.ReturnsAsync(clientData);
		var handler = new CreateOrderEventHandler(_mockMediator.Object, _mockClientDataRepository.Object,
			_mockCustomEmailSender.Object, _mockCreateOrderEmailContent.Object, _mockOrderTrackingUrl.Object);

        _mockMediator.Setup(x => x.Send(It.IsAny<CreateOrderCommand>(),CancellationToken.None))
            .ReturnsAsync(() => new (expectedOrderId, expectedShortId, expectedAddedDate));
		// act
		await handler.Handle(@event, CancellationToken.None);

		// assert
		_mockMediator.Verify(x => x.Send(It.Is<CreateOrderCommand>(
			x => x.Description == @event.Description && x.ClientDataId == clientData.Id), CancellationToken.None), Times.Once);
		_mockMediator.Verify(x => x.Send(It.Is<CreateClientDataCommand>(
			x => x.PhoneNumber == @event.PhoneNumber && x.Email == @event.Email), CancellationToken.None), Times.Never);
	}
}
