using BikeWorkshop.Domain.Entities;

namespace BikeWorkshop.Application.Functions.DTO;

public record OrderDto
{
    public Guid OrderId { get; init; }
	public string Description { get; init; }
	public DateTime AddedDate { get; init; }
	public string ShortUniqueId { get; init; }
	internal static List<OrderDto> TranslateListOrderToDto(List<Order> orders) 
		=> orders.Select(x => new OrderDto()
			{
				OrderId = x.Id,
				AddedDate = x.AddedDate,
				Description = x.Description,
				ShortUniqueId = x.ShortUniqueId
			}).ToList();
}
