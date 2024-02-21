using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Application.Interfaces.Services;
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
	private readonly IClientDataRepository _clientDataRepository;
	private readonly ICustomEmailSender _customEmailSender;
	private readonly ISummaryEmailContent _summaryEmailContent;

	public CreateSummaryForOrderCommandHandler(
		IOrderRepository orderRepository,
		ISummaryRepository summaryRepository,
		IServiceToOrderRepository serviceToOrderRepository,
		IClientDataRepository clientDataRepository,
		ICustomEmailSender customEmailSender,
		ISummaryEmailContent summaryEmailContent)
	{
		_orderRepository = orderRepository;
		_summaryRepository = summaryRepository;
		_serviceToOrderRepository = serviceToOrderRepository;
		_clientDataRepository = clientDataRepository;
		_customEmailSender = customEmailSender;
		_summaryEmailContent = summaryEmailContent;
	}

	public async Task Handle(CreateSummaryForOrderCommand request, CancellationToken cancellationToken)
	{
		var order = await _orderRepository.GetById(request.OrderId)
			?? throw new NotFoundException("Unknown order!");
		var clientEmail = await _clientDataRepository.GetEmailByOrderId(request.OrderId);
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
		if(clientEmail is not null)
		{
			await _customEmailSender.SendEmailAsync(clientEmail, "Summary of order",_summaryEmailContent.Content(totalAmmount));
		}
	}
}
