using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Functions.DTO.Enums;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Application.Pagination;
using MediatR;

namespace BikeWorkshop.Application.Functions.OrderFunctions.Queries.GePageOfCompleted;
internal class GetPageOfCompletedQueryHandler
	: IRequestHandler<GetPageOfCompletedQuery, PagedList<OrderDto>>
{
	private readonly IOrderRepository _orderRepository;

	public GetPageOfCompletedQueryHandler(IOrderRepository orderRepository)
	{
		_orderRepository = orderRepository;
	}

	public async Task<PagedList<OrderDto>> Handle(GetPageOfCompletedQuery request, CancellationToken cancellationToken)
	{
		var orders = await _orderRepository.GetAllCompleted();
		var sortedOrders = request.SortingDirection switch
		{
			SortingDirection.Ascending => orders.OrderBy(x => x.AddedDate).ToList(),
			SortingDirection.Descending => orders.OrderByDescending(x => x.AddedDate).ToList(),
			_ => orders,
		};
		var dtos = OrderDto.TranslateListOrderToDto(sortedOrders);
		return PagedList<OrderDto>.Create(dtos, request.Page,request.PageSize);
	}
}
