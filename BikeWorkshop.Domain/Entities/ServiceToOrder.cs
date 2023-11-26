namespace BikeWorkshop.Domain.Entities;

public class ServiceToOrder
{
	public Guid Id { get; set; }
	public decimal Price { get; set; }
	public int Count { get; set; }
    public Guid ServiceId { get; set; }
    public Service Service { get; set; }
    public Guid OrderId { get; set; }
    public Order Order { get; set; }
}
