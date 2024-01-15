using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Shared.Exceptions;
using MediatR;

namespace BikeWorkshop.Application.Functions.ServiceToOrderFunctions.Queries.GetByOrder;

internal class GetServiceToOrderByOrderQueryHandler
	: IRequestHandler<GetServiceToOrderByOrderQuery, List<ServiceToOrderDto>>
{
	private readonly IServiceToOrderRepository _serviceToOrderRepository;
	private readonly IOrderRepository _orderRepository;
	public GetServiceToOrderByOrderQueryHandler(IServiceToOrderRepository serviceToOrderRepository, IOrderRepository orderRepository)
	{
		_serviceToOrderRepository = serviceToOrderRepository;
		_orderRepository = orderRepository;
	}

	public async Task<List<ServiceToOrderDto>> Handle(GetServiceToOrderByOrderQuery request, CancellationToken cancellationToken)
	{
		var order = await _orderRepository.GetById(request.OrderId)
			?? throw new NotFoundException("Unknown order!");	
		return ServiceToOrderDto.TranslateList(
				await _serviceToOrderRepository.GetByOrderId(request.OrderId)
				).OrderBy(x => x.Price)
			.ToList();
	}	
}
