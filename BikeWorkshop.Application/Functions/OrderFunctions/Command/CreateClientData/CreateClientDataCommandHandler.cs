using BikeWorkshop.Application.Fluent_Validation_Extensions;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Shared.Exceptions;
using FluentValidation;
using MediatR;

namespace BikeWorkshop.Application.Functions.OrderFunctions.Command.CreateClientData;

internal sealed class CreateClientDataCommandHandler : IRequestHandler<CreateClientDataCommand, Guid>
{
	private readonly IClientDataRepository _clientDataRepository;
	private readonly IValidator<CreateClientDataCommand> _validator;

	public CreateClientDataCommandHandler(
		IClientDataRepository clientDataRepository,
		IValidator<CreateClientDataCommand> validator)
	{
		_clientDataRepository = clientDataRepository;
		_validator = validator;
	}

	public async Task<Guid> Handle(CreateClientDataCommand request, CancellationToken cancellationToken)
	{
		var valResult = await _validator.ValidateAsync(request, cancellationToken);
		if (!valResult.IsValid)
		{
			throw new BadRequestException(valResult.Errors.ToJsonString());
		}
		var clientData = new ClientData
		{
			Id = Guid.NewGuid(),
			Email = request.Email,
			PhoneNumber = request.PhoneNumber,
		};
		await _clientDataRepository.Add(clientData);
		return clientData.Id;
	}
}
