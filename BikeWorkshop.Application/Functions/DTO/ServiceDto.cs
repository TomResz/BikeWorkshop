using BikeWorkshop.Domain.Entities;

namespace BikeWorkshop.Application.Functions.DTO;

public class ServiceDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }

    internal static List<ServiceDto> TranslateOrdersToDtos(List<Service> orders)
        => orders.Select(x=>new ServiceDto()
        {
            Id = x.Id,
            Name = x.Name,
        }).ToList();
}
