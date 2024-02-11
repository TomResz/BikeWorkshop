using BikeWorkshop.Application.Functions.DTO;
using MediatR;

namespace BikeWorkshop.Application.Functions.SummaryFunctions.Queries.GetSummaryWithDetails;
public record GetSummaryWithDetailsQuery(
	Guid OrderId) : IRequest<SummaryWithDetailsDto>;
