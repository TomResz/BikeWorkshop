using MediatR;

namespace BikeWorkshop.Application.Functions.OrderFunctions.Command.CreateOrder;

public sealed record CreateOrderCommand(
	string Description,
	Guid ClientDataId) : IRequest<CreateOrderResponse>;
