using MediatR;

namespace BikeWorkshop.Application.Functions.SummaryFunctions.Command.Delete;
public record DeleteSummaryCommand(
	Guid OrderId) : IRequest;