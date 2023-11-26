namespace BikeWorkshop.Domain.Entities;

public class ClientData
{
    public Guid Id { get; set; }
    public string? Email { get; set; }
    public string PhoneNumber { get; set; }
    public ICollection<Order> Orders { get; set; }
}
