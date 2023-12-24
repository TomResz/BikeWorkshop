using MediatR;

namespace BikeWorkshop.Application.Functions.ServiceFunctions.Commands.Update;

public record UpdateServiceCommand(
	Guid ServiceId,
	string Name) : IRequest;
