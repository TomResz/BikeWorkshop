using MediatR;

namespace BikeWorkshop.Application.Functions.SummaryFunctions.Command.CreateSummaryForOrder;
public sealed record CreateSummaryForOrderCommand(
	Guid OrderId,
	string? Conclusion=null) : IRequest;
