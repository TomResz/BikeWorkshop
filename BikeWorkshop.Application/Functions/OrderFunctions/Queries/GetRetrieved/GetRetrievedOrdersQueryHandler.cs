using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Functions.DTO.Enums;
using BikeWorkshop.Application.Interfaces.Repositories;
using MediatR;

namespace BikeWorkshop.Application.Functions.OrderFunctions.Queries.GetRetrieved;
internal sealed class GetRetrievedOrdersQueryHandler
	: IRequestHandler<GetRetrievedOrdersQuery, List<OrderDto>>
{
	private readonly IOrderRepository _orderRepository;

	public GetRetrievedOrdersQueryHandler(IOrderRepository orderRepository)
	{
		_orderRepository = orderRepository;
	}

	public async Task<List<OrderDto>> Handle(GetRetrievedOrdersQuery request, CancellationToken cancellationToken)
	{
		var orders = await _orderRepository.GetAllRetrieved();
		var sortedOrders = request.Direction switch
		{
			SortingDirection.Ascending => orders.OrderBy(x => x.AddedDate).ToList(),
			SortingDirection.Descending => orders.OrderByDescending(x => x.AddedDate).ToList(),
			_ => orders,
		};
		return OrderDto.TranslateListOrderToDto(sortedOrders);
	}
}
