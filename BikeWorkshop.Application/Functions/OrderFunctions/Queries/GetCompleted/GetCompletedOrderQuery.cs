using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Functions.DTO.Enums;
using MediatR;

namespace BikeWorkshop.Application.Functions.OrderFunctions.Queries.GetCompleted;

public record GetCompletedOrderQuery(
	SortingDirection Direction) : IRequest<List<OrderDto>>;
