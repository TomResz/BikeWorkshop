using MediatR;

namespace BikeWorkshop.Application.Functions.OrderFunctions.Events.CreateOrder;

public sealed record CreateOrderEvent(
	string? Email,
	string PhoneNumber,
	string Description) : IRequest;

