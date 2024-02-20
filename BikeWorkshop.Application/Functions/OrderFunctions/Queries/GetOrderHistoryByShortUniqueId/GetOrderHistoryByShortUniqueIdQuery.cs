using BikeWorkshop.Application.Functions.DTO;
using MediatR;

namespace BikeWorkshop.Application.Functions.OrderFunctions.Queries.GetOrderHistoryByShortUniqueId;
public record GetOrderHistoryByShortUniqueIdQuery(string ShortUniqueId) : IRequest<OrderHistoryDto>;
