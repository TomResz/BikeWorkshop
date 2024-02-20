using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Enums;
using BikeWorkshop.Shared.Exceptions;
using MediatR;

namespace BikeWorkshop.Application.Functions.OrderFunctions.Command.RetrieveOrder;
internal sealed class RetrieveOrderCommandHandler
	: IRequestHandler<RetrieveOrderCommand>
{
	private readonly IOrderRepository _orderRepository;

	public RetrieveOrderCommandHandler(IOrderRepository orderRepository)
	{
		_orderRepository = orderRepository;
	}

	public async Task Handle(RetrieveOrderCommand request, CancellationToken cancellationToken)
	{
		var order = await _orderRepository.GetById(request.OrderId)
			?? throw new NotFoundException("Unknown order!");
		if(order.OrderStatusId is not (int)Status.Completed)
		{
			throw new BadRequestException("You can't retrieve not completed order!");
		}
		order.OrderStatusId = (int)Status.Retrieved;
		await _orderRepository.Update(order);
	}
}
