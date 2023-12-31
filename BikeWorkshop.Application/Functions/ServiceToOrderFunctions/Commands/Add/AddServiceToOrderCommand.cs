using MediatR;

namespace BikeWorkshop.Application.Functions.ServiceToOrderFunctions.Commands.Add;

public sealed record AddServiceToOrderCommand(
	Guid ServiceId,
	Guid OrderId,
	int Count,
	decimal Price) : IRequest;
