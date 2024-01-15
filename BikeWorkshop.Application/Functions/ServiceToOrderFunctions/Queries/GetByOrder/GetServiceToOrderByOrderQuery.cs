using BikeWorkshop.Application.Functions.DTO;
using MediatR;

namespace BikeWorkshop.Application.Functions.ServiceToOrderFunctions.Queries.GetByOrder;

public sealed record GetServiceToOrderByOrderQuery(
	Guid OrderId) : IRequest<List<ServiceToOrderDto>>;
