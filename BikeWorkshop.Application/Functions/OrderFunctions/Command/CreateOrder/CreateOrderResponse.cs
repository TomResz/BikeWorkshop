namespace BikeWorkshop.Application.Functions.OrderFunctions.Command.CreateOrder;

public record CreateOrderResponse(
	Guid OrderId,
	string ShortUniqueId,
	DateTime AddedDate);