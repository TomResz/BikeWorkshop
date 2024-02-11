using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Domain.Enums;
using BikeWorkshop.Shared.Exceptions;
using MediatR;

namespace BikeWorkshop.Application.Functions.SummaryFunctions.Command.CreateSummaryForOrder;
internal sealed class CreateSummaryForOrderCommandHandler
	: IRequestHandler<CreateSummaryForOrderCommand>
{
	private readonly IOrderRepository _orderRepository;
	private readonly ISummaryRepository _summaryRepository;
	private readonly IServiceToOrderRepository _serviceToOrderRepository;
	public CreateSummaryForOrderCommandHandler(
		IOrderRepository orderRepository,
		ISummaryRepository summaryRepository,
		IServiceToOrderRepository serviceToOrderRepository)
	{
		_orderRepository = orderRepository;
		_summaryRepository = summaryRepository;
		_serviceToOrderRepository = serviceToOrderRepository;
	}

	public async Task Handle(CreateSummaryForOrderCommand request, CancellationToken cancellationToken)
	{
		var order = await _orderRepository.GetById(request.OrderId)
			?? throw new NotFoundException("Unknown order!");

		if(order.OrderStatusId is not (int)Status.During)
		{
			throw new BadRequestException("This order has already ended!");
		}

		order.OrderStatusId = (int)Status.Completed;

		var services = await _serviceToOrderRepository.GetByOrderId(request.OrderId);
		var totalAmmount = services.Sum(x => x.Price * x.Count);

		var summary = new Summary
		{
			Id = Guid.NewGuid(),
			TotalPrice = totalAmmount,
			OrderId = request.OrderId,
			EndedDate = DateTime.Now,
			Conclusion = request.Conclusion
		};
		await _orderRepository.Update(order);
		await _summaryRepository.Add(summary);
	}
}
