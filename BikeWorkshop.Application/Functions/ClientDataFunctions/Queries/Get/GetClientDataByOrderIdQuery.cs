using BikeWorkshop.Application.Functions.DTO;
using MediatR;

namespace BikeWorkshop.Application.Functions.ClientDataFunctions.Queries.Get;
public record GetClientDataByOrderIdQuery(
    Guid OrderId) : IRequest<ClientDataDto>;
