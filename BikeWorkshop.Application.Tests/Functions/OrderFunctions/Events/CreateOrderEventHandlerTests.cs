using BikeWorkshop.Application.Functions.OrderFunctions.Command.CreateClientData;
using BikeWorkshop.Application.Functions.OrderFunctions.Command.CreateOrder;
using BikeWorkshop.Application.Functions.OrderFunctions.Events.CreateOrder;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using MediatR;
using Moq;

namespace BikeWorkshop.Application.Tests.Functions.OrderFunctions.Events;

public class CreateOrderEventHandlerTests
{
	private readonly Mock<IMediator> _mockMediator;
	private readonly Mock<IClientDataRepository> _mockClientDataRepository;

    public CreateOrderEventHandlerTests()
    {
        _mockClientDataRepository = new();
        _mockMediator = new();
    }


    [Fact]
    public async Task Handle_WithUnknownCustomer_Should_CreateClientData()
    {
        // arrange
        var @event = new CreateOrderEvent("tomasz@gmail.com", "+48999777555", "Example description.");
        _mockClientDataRepository.Setup(rep => rep.GetByPhoneNumberOrEmail(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((string email, string phoneNumber) => null);
        var handler = new CreateOrderEventHandler(_mockMediator.Object, _mockClientDataRepository.Object);

        // act
        await handler.Handle(@event, CancellationToken.None);

        // assert
        _mockMediator.Verify(x=>x.Send(It.Is<CreateClientDataCommand>(
            x=>x.PhoneNumber == @event.PhoneNumber && x.Email == @event.Email), CancellationToken.None),Times.Once);
        _mockMediator.Verify(x => x.Send(It.Is<CreateOrderCommand>(
            x => x.Description == @event.Description), CancellationToken.None),Times.Once);
    }
	[Fact]
	public async Task Handle_WithExistingCustomer_Should_OnlyCreateOrder()
	{
		// arrange
        var clientData = new ClientData()
        {
            Id = Guid.NewGuid(),
            Email = "tomasz@gmail.com",
            PhoneNumber = "48999777555"
		};

		var @event = new CreateOrderEvent(clientData.Email, clientData.PhoneNumber, "Example description.");
		_mockClientDataRepository.Setup(rep => rep.GetByPhoneNumberOrEmail(It.IsAny<string>(), It.IsAny<string>()))
			.ReturnsAsync(clientData);
		var handler = new CreateOrderEventHandler(_mockMediator.Object, _mockClientDataRepository.Object);

		// act
		await handler.Handle(@event, CancellationToken.None);

		// assert
		_mockMediator.Verify(x => x.Send(It.Is<CreateClientDataCommand>(
			x => x.PhoneNumber == @event.PhoneNumber && x.Email == @event.Email), CancellationToken.None), Times.Never);
		_mockMediator.Verify(x => x.Send(It.Is<CreateOrderCommand>(
			x => x.Description == @event.Description && x.ClientDataId == clientData.Id), CancellationToken.None), Times.Once);
	}
}
