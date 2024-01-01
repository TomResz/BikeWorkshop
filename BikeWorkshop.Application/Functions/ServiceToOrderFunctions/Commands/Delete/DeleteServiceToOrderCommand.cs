using MediatR;

namespace BikeWorkshop.Application.Functions.ServiceToOrderFunctions.Commands.Delete;

public record DeleteServiceToOrderCommand(
	Guid ServiceToOrderId) : IRequest;
