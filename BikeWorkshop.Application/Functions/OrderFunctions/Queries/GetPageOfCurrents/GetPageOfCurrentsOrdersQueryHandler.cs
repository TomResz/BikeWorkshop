using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Application.Pagination;
using MediatR;

namespace BikeWorkshop.Application.Functions.OrderFunctions.Queries.GetPageOfCurrents;
internal class GetPageOfCurrentsOrdersQueryHandler
	: IRequestHandler<GetPageOfCurrentsOrdersQuery, PagedList<OrderDto>>
{
	private readonly IOrderRepository _orderRepository;

	public GetPageOfCurrentsOrdersQueryHandler(IOrderRepository orderRepository)
	{
		_orderRepository = orderRepository;
	}

	public async Task<PagedList<OrderDto>> Handle(GetPageOfCurrentsOrdersQuery request, CancellationToken cancellationToken)
	{
		var list = await _orderRepository.GetAllActive();
		return PagedList<OrderDto>.Create(OrderDto.TranslateListOrderToDto(list)
			,request.Page,request.PageSize);
	}
}
