using BikeWorkshop.Domain.Entities;

namespace BikeWorkshop.Application.Functions.DTO;

public record ServiceToOrderDto(
    Guid Id,
    decimal Price,
    int Count)
{
    internal static List<ServiceToOrderDto> TranslateList(List<ServiceToOrder> services)
        => services.Select(x => new ServiceToOrderDto(
            x.Id, 
            x.Price, 
            x.Count))
        .ToList();
}
