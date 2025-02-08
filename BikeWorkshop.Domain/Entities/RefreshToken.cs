namespace BikeWorkshop.Domain.Entities;
public class RefreshToken
{
    public Guid Id { get; set; }
    public DateTime ExpirationTimeUtc { get; set; }
    public string Token { get; set; }
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; }
}
