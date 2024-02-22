using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Functions.DTO.Enums;
using BikeWorkshop.Application.Pagination;
using MediatR;

namespace BikeWorkshop.Application.Functions.OrderFunctions.Queries.GetPageOfRetrieved;
public record GetPageOfRetrievedOrdersQuery(
	int Page,
	int PageSize,
	SortingDirection? SortingDirection = null) : IRequest<PagedList<OrderDto>>;
