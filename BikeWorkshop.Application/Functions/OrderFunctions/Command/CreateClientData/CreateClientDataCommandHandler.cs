using BikeWorkshop.Application.Fluent_Validation_Extensions;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Shared.Exceptions;
using FluentValidation;
using MediatR;

namespace BikeWorkshop.Application.Functions.OrderFunctions.Command.CreateClientData;

internal sealed class CreateClientDataCommandHandler : IRequestHandler<CreateClientDataCommand, ClientData>
{
	private readonly IClientDataRepository _clientDataRepository;

	public CreateClientDataCommandHandler(
		IClientDataRepository clientDataRepository)
	{
		_clientDataRepository = clientDataRepository;
	}

	public async Task<ClientData> Handle(CreateClientDataCommand request, CancellationToken cancellationToken)
	{
		var clientData = new ClientData
		{
			Id = Guid.NewGuid(),
			Email = request.Email,
			PhoneNumber = request.PhoneNumber,
		};
		await _clientDataRepository.Add(clientData);
		return clientData;
	}
}
