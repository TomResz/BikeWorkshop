using BikeWorkshop.Domain.Enums;

namespace BikeWorkshop.Application.Functions.DTO;
public class OrderHistoryDto
{
	public string OrderName { get; set; }
	public DateTime AddedDate { get; set; }
	public string ActualStatus { get; set; }
    public IEnumerable<StatusHistory> StatusHistoryDtos { get; set; }
}
