using BikeWorkshop.Domain.Entities;

namespace BikeWorkshop.Application.Functions.DTO;
public class SummaryWithDetailsDto
{
	public string? Conclusion { get; set; }
    public decimal TotalPrice { get;  set; }
    public DateTime EndedDate { get;  set; }
    public List<ServiceDetailsDto> ServiceDetails { get;  set; }

	public static SummaryWithDetailsDto Translate(Summary summary,List<ServiceToOrder> services)
		=> new()
		{
			Conclusion = summary.Conclusion,
			EndedDate = summary.EndedDate,
			TotalPrice = summary.TotalPrice,
			ServiceDetails = services.Select(x => new ServiceDetailsDto
			{
				Count = x.Count,
				Name = x.Service.Name,
				Price = x.Price,
			}).ToList()
		};
}
