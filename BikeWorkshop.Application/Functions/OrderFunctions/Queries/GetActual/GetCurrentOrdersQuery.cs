using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Functions.DTO.Enums;
using MediatR;

namespace BikeWorkshop.Application.Functions.OrderFunctions.Queries.GetActual;

public record GetCurrentOrdersQuery(
	SortingDirection Direction) : IRequest<List<OrderDto>>;
