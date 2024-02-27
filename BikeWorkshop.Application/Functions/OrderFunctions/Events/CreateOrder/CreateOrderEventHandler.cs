using BikeWorkshop.Application.Functions.ClientDataFunctions.Commands.CreateClientData;
using BikeWorkshop.Application.Functions.OrderFunctions.Command.CreateOrder;
using BikeWorkshop.Application.Interfaces.Email;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Application.Interfaces.Services;
using MediatR;

namespace BikeWorkshop.Application.Functions.OrderFunctions.Events.CreateOrder;

internal sealed class CreateOrderEventHandler : IRequestHandler<CreateOrderEvent>
{
	private readonly IMediator _mediator;
	private readonly IClientDataRepository _clientDataRepository;
	private readonly ICustomEmailSender _customEmailSender;
	private readonly ICreateOrderEmailContent _createOrderEmailContent;
	private readonly IOrderTrackingURL _createOrderTrackingURL;
	public CreateOrderEventHandler(
		IMediator mediator,
		IClientDataRepository clientDataRepository,
		ICustomEmailSender customEmailSender,
		ICreateOrderEmailContent createOrderEmailContent,
		IOrderTrackingURL createOrderTrackingURL)
	{
		_mediator = mediator;
		_clientDataRepository = clientDataRepository;
		_customEmailSender = customEmailSender;
		_createOrderEmailContent = createOrderEmailContent;
		_createOrderTrackingURL = createOrderTrackingURL;
	}

	public async Task Handle(CreateOrderEvent @event, CancellationToken cancellationToken)
	{
		Guid clientId;
		var clientData = await _clientDataRepository.GetByPhoneNumberOrEmail(@event.PhoneNumber, @event.Email);
		if (clientData is null)
		{
			clientData= await _mediator.Send(new CreateClientDataCommand(@event.Email, @event.PhoneNumber));
			clientId = clientData.Id;
		}
		else
		{
			clientId = clientData.Id;
		}
		var orderResponse = await _mediator.Send(new CreateOrderCommand(@event.Description, clientId));
		var (orderId,shortId,addedDate) = orderResponse;
		if(@event.Email is not null)
		{
			var content = _createOrderEmailContent.Content(_createOrderTrackingURL.Url,shortId);
			await _customEmailSender.SendEmailAsync(@event.Email, "Order has been already created!", content);
		}
	}
}
