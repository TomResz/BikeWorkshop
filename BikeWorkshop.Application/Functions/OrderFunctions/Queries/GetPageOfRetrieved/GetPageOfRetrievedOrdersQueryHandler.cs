using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Functions.DTO.Enums;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Application.Pagination;
using MediatR;

namespace BikeWorkshop.Application.Functions.OrderFunctions.Queries.GetPageOfRetrieved;
internal class GetPageOfRetrievedOrdersQueryHandler
	: IRequestHandler<GetPageOfRetrievedOrdersQuery, PagedList<OrderDto>>
{
	private readonly IOrderRepository _orderRepository;

	public GetPageOfRetrievedOrdersQueryHandler(IOrderRepository orderRepository)
	{
		_orderRepository = orderRepository;
	}

	public async Task<PagedList<OrderDto>> Handle(GetPageOfRetrievedOrdersQuery request, CancellationToken cancellationToken)
	{
		var orders = await _orderRepository.GetAllRetrieved();
		var sortedOrders = request.SortingDirection switch
		{
			SortingDirection.Ascending => orders.OrderBy(x => x.AddedDate).ToList(),
			SortingDirection.Descending => orders.OrderByDescending(x => x.AddedDate).ToList(),
			_ => orders,
		};
		var dtos =  OrderDto.TranslateListOrderToDto(sortedOrders);
		return PagedList<OrderDto>.Create(dtos,request.Page, request.PageSize);
	}
}
