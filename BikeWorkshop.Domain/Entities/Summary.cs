namespace BikeWorkshop.Domain.Entities;

public class Summary
{
    public Guid Id { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime EndedDate { get; set; }
    public string? Conclusion { get; set; }
    public Guid OrderId { get; set; }
    public Order Order { get; set; }
}
