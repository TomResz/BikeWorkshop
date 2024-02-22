using BikeWorkshop.Application.Functions.DTO;
using MediatR;

namespace BikeWorkshop.Application.Functions.SummaryFunctions.Queries.GetSummaryWithDetailsByShortId;
public record GetSummaryWithDetailsByShortIdQuery(
	string ShortId) : IRequest<SummaryWithDetailsDto>;

