using BikeWorkshop.Application.Functions.OrderFunctions.Command.CreateClientData;
using BikeWorkshop.Application.Functions.OrderFunctions.Command.CreateOrder;
using BikeWorkshop.Application.Interfaces.Repositories;
using MediatR;

namespace BikeWorkshop.Application.Functions.OrderFunctions.Events.CreateOrder;

internal sealed class CreateOrderEventHandler : IRequestHandler<CreateOrderEvent>
{
	private readonly IMediator _mediator;
	private readonly IClientDataRepository _clientDataRepository;
	public CreateOrderEventHandler(IMediator mediator, IClientDataRepository clientDataRepository)
	{
		_mediator = mediator;
		_clientDataRepository = clientDataRepository;
	}

	public async Task Handle(CreateOrderEvent @event, CancellationToken cancellationToken)
	{
		Guid clientId;
		var clientData = await _clientDataRepository.GetByPhoneNumberOrEmail(@event.PhoneNumber, @event.Email);
		if (clientData is null)
		{
			clientId = await _mediator.Send(new CreateClientDataCommand(@event.Email, @event.PhoneNumber));
		}
		else
		{
			clientId = clientData.Id;
		}
		await _mediator.Send(new CreateOrderCommand(@event.Description, clientId));
	}
}
