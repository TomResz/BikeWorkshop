using MediatR;

namespace BikeWorkshop.Application.Functions.ServiceFunctions.Commands.Delete;

public record DeleteServiceCommand(
	Guid ServiceId) : IRequest;
