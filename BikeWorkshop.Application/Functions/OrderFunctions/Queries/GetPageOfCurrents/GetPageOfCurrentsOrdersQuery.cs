using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Pagination;
using MediatR;

namespace BikeWorkshop.Application.Functions.OrderFunctions.Queries.GetPageOfCurrents;
public record GetPageOfCurrentsOrdersQuery(
	int Page,
	int PageSize) : IRequest<PagedList<OrderDto>>;
