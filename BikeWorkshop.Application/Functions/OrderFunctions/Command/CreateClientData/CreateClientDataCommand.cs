using MediatR;

namespace BikeWorkshop.Application.Functions.OrderFunctions.Command.CreateClientData;

public record CreateClientDataCommand(
	string? Email,
	string PhoneNumber) : IRequest<Guid>;
