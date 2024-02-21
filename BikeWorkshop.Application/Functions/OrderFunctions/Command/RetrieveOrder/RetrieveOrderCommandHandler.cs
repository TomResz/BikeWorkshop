using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Enums;
using BikeWorkshop.Shared.Exceptions;
using MediatR;

namespace BikeWorkshop.Application.Functions.OrderFunctions.Command.RetrieveOrder;
internal sealed class RetrieveOrderCommandHandler
	: IRequestHandler<RetrieveOrderCommand>
{
	private readonly IOrderRepository _orderRepository;
	private readonly ISummaryRepository _summaryRepository;
	public RetrieveOrderCommandHandler(
		IOrderRepository orderRepository,
		ISummaryRepository summaryRepository)
	{
		_orderRepository = orderRepository;
		_summaryRepository = summaryRepository;
	}

	public async Task Handle(RetrieveOrderCommand request, CancellationToken cancellationToken)
	{
		var order = await _orderRepository.GetById(request.OrderId)
			?? throw new NotFoundException("Unknown order Id!");
		var summary = await _summaryRepository.GetByOrderId(request.OrderId)
			?? throw new NotFoundException("Unknown order Id!");
		if (order.OrderStatusId is not (int)Status.Completed)
		{
			throw new BadRequestException("You can't retrieve not completed order!");
		}
		order.OrderStatusId = (int)Status.Retrieved;
		summary.RetrievedDate = DateTime.Now;
		await _orderRepository.Update(order);
		await _summaryRepository.Update(summary);	
	}
}
