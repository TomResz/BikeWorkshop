using BikeWorkshop.Application.Fluent_Validation_Extensions;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Application.Interfaces.Services;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Shared.Exceptions;
using FluentValidation;
using MediatR;

namespace BikeWorkshop.Application.Functions.OrderFunctions.Command.CreateOrder;

internal sealed class CreateOrderCommandHandler 
	: IRequestHandler<CreateOrderCommand, CreateOrderResponse>
{
	private readonly IOrderRepository _orderRepository;
	private readonly IEmployeeSessionContext _employeeSessionContext;
	private readonly IShortIdService _shortIdService;
	public CreateOrderCommandHandler(
		IOrderRepository orderRepository,
		IEmployeeSessionContext employeeSessionContext,
		IShortIdService shortIdService)
	{
		_orderRepository = orderRepository;
		_employeeSessionContext = employeeSessionContext;
		_shortIdService = shortIdService;
	}

	public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
	{
		var employeeId = _employeeSessionContext.GetEmployeeId() ??
			throw new UnauthorizedException(nameof(CreateOrderCommandHandler));
		var order = new Order
		{
			Id = Guid.NewGuid(),
			AddedDate = DateTime.UtcNow,
			Description = request.Description,
			ClientDataId = request.ClientDataId,
			ShortUniqueId = _shortIdService.GetShortId(),
			EmployeeId = employeeId,
		};
		await _orderRepository.Add(order);
		return new(order.Id, order.ShortUniqueId, order.AddedDate);
	}
}
