using MediatR;

namespace BikeWorkshop.Application.Functions.OrderFunctions.Command.RetrieveOrder;
public record RetrieveOrderCommand(Guid OrderId) : IRequest;
