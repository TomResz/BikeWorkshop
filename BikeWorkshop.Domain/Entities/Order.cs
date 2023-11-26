using BikeWorkshop.Domain.Enums;

namespace BikeWorkshop.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public DateTime AddedDate { get; set; }
    public string ShortUniqueId { get; set; }
    
    public int OrderStatusId { get; set; } = (int)Status.During;
    public OrderStatus OrderStatus { get; set; }

	public Guid EmployeeId { get; set; }
	public Employee Employee { get; set; }

    public Guid ClientDataId { get; set; }
    public ClientData ClientData { get; set; }

    public Guid SummaryId { get; set; }
    public Summary Summary { get; set; }
}
