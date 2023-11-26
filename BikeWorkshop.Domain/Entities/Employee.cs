using BikeWorkshop.Domain.Enums;

namespace BikeWorkshop.Domain.Entities;

public class Employee
{
    public Guid Id { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Email { get; set; }
    public string PasswordHash { get; set; }
    public int RoleId { get; set; } = (int)Roles.Worker;
    public Role Role { get; set; }
}
