using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Functions.DTO.Enums;
using MediatR;

namespace BikeWorkshop.Application.Functions.OrderFunctions.Queries.GetRetrieved;
public record GetRetrievedOrdersQuery(
	SortingDirection Direction) : IRequest<List<OrderDto>>;
