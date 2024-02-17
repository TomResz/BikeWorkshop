namespace BikeWorkshop.Application.Functions.DTO;
public class SummaryWithDetailsDto
{
	public string? Conclusion { get; set; }
    public decimal TotalPrice { get;  set; }
    public DateTime EndedDate { get;  set; }
    public List<ServiceDetailsDto> ServiceDetails { get;  set; }
}
