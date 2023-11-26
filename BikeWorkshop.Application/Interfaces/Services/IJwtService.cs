using BikeWorkshop.Domain.Entities;

namespace BikeWorkshop.Application.Interfaces.Services;

public interface IJwtService
{
	string GetToken(Employee employee);
}
