using BikeWorkshop.Application.Functions.DTO;
using MediatR;

namespace BikeWorkshop.Application.Functions.ClientDataFunctions.Queries.GetByShortId;
public record GetClientDataByShortIdQuery(
	string ShortId) : IRequest<ClientDataDto>;