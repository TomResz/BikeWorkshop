using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Domain.Enums;
using BikeWorkshop.Shared.Exceptions;
using MediatR;

namespace BikeWorkshop.Application.Functions.OrderFunctions.Queries.GetOrderHistoryByShortUniqueId;
internal sealed class GetOrderHistoryByShortUniqueIdQueryHandler
	: IRequestHandler<GetOrderHistoryByShortUniqueIdQuery, OrderHistoryDto>
{
	private readonly IOrderRepository _orderRepository;

	public GetOrderHistoryByShortUniqueIdQueryHandler(IOrderRepository orderRepository)
	{
		_orderRepository = orderRepository;
	}

	public async Task<OrderHistoryDto> Handle(GetOrderHistoryByShortUniqueIdQuery request, CancellationToken cancellationToken)
	{
		var order = await _orderRepository.GetByShortId(request.ShortUniqueId) ??
			throw new NotFoundException("Unknown order unique Id!");

		var orderHistoryList = new List<StatusHistory>()
		{
			new StatusHistory()
			{
				Date = order.AddedDate,
				Value = "Order added.",
				Description = @$"Your order: {order.Description} has been added."
			}
		};

		switch (order.OrderStatusId)
		{
			case (int)Status.Retrieved:
				{
					orderHistoryList.Add(GetCompletedHistoryDto(order));
					orderHistoryList.Add(new StatusHistory()
					{
						Date = order.Summary.RetrievedDate,
						Description = order.Summary.Conclusion ?? "",
						Value = @$"Your order: {order.Description} has been retrieved!"
					});
				}
				break;
			case (int)Status.Completed:
				{
					orderHistoryList.Add(GetCompletedHistoryDto(order));
				}
				break;
		}
		return new()
		{
			ActualStatus = ConvertStatusIntToString(order.OrderStatusId),
			AddedDate = order.AddedDate,
			OrderName = order.Description,
			StatusHistoryDtos = orderHistoryList
		};
	}
	private string ConvertStatusIntToString(int status) 
	{
		if(Enum.IsDefined(typeof(Status), status)) 
			return Enum.GetName(typeof(Status), status) ?? throw new BadRequestException("Invalid status Id!");
		throw new BadRequestException("Invalid status Id!");
	}
	private StatusHistory GetCompletedHistoryDto(Order order)
		=> new StatusHistory()
		{
			Date = order.Summary.EndedDate,
			Description = order.Summary.Conclusion ?? "",
			Value = @$"Your order: ""{order.Description}"" is ready to retrieve!"
		};
}
