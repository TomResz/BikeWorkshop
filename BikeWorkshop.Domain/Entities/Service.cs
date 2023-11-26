namespace BikeWorkshop.Domain.Entities;

public class Service
{
	public Guid Id { get; set; }
	public string Name { get; set; }
    public ICollection<ServiceToOrder> ServiceToOrders { get; set; }
}
