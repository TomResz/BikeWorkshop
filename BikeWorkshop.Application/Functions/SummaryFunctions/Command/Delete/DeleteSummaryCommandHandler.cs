using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Enums;
using BikeWorkshop.Shared.Exceptions;
using MediatR;

namespace BikeWorkshop.Application.Functions.SummaryFunctions.Command.Delete;
internal class DeleteSummaryCommandHandler
	: IRequestHandler<DeleteSummaryCommand>
{
	private readonly ISummaryRepository _summaryRepository;
	private readonly IOrderRepository _orderRepository;
	public DeleteSummaryCommandHandler(ISummaryRepository summaryRepository, IOrderRepository orderRepository)
	{
		_summaryRepository = summaryRepository;
		_orderRepository = orderRepository;
	}

	public async Task Handle(DeleteSummaryCommand request, CancellationToken cancellationToken)
	{
		var summary = await _summaryRepository.GetByOrderId(request.OrderId)
			?? throw new NotFoundException("Unknown order!");

		if (summary.Order.OrderStatusId is (int)Status.Retrieved)
		{
			throw new BadRequestException("You can't delete summary of completed order!");
		}
		summary.Order.OrderStatusId = (int)Status.During;
		await _orderRepository.Update(summary.Order);
		await _summaryRepository.Delete(summary);
	}
}
