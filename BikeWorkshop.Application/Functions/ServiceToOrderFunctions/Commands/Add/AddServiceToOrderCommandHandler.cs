using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Domain.Enums;
using BikeWorkshop.Shared.Exceptions;
using MediatR;

namespace BikeWorkshop.Application.Functions.ServiceToOrderFunctions.Commands.Add;

internal sealed class AddServiceToOrderCommandHandler
	: IRequestHandler<AddServiceToOrderCommand>
{
	private readonly IServiceRepository _serviceRepository;
	private readonly IOrderRepository _orderRepository;
	private readonly IServiceToOrderRepository _serviceToOrderRepository;

	public AddServiceToOrderCommandHandler(
		IServiceRepository serviceRepository,
		IOrderRepository orderRepository,
		IServiceToOrderRepository serviceToOrderRepository)
	{
		_serviceRepository = serviceRepository;
		_orderRepository = orderRepository;
		_serviceToOrderRepository = serviceToOrderRepository;
	}

	public async Task Handle(AddServiceToOrderCommand request, CancellationToken cancellationToken)
	{
		var order = await _orderRepository.GetById(request.OrderId);
		if(order is null)
		{
			throw new NotFoundException($"{nameof(order)} not found!");
		}

		if(order.OrderStatusId != (int)Status.During)
		{
			throw new BadRequestException("This order is already completed or retrieved!");
		}

		var service = await _serviceRepository.GetById(request.ServiceId);
		if(service is null)
		{
			throw new BadRequestException($"Service with {request.ServiceId} was not found!");
		}

		var serviceToOrder = new ServiceToOrder
		{
			Count = request.Count,
			Id = Guid.NewGuid(),
			Price = request.Price,
			OrderId	= request.OrderId,
			ServiceId = request.ServiceId
		};
		await _serviceToOrderRepository.Add(serviceToOrder);
	}
}
