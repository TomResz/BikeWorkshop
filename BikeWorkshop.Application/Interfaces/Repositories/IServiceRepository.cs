using BikeWorkshop.Domain.Entities;

namespace BikeWorkshop.Application.Interfaces.Repositories;

public interface IServiceRepository
{
	Task<bool> Add(Service service);
	Task Delete(Service service);
	Task<List<Service>> GetAll();
	Task<Service?> GetById(Guid serviceId);
	Task<Service?> GetByName(string name);
	Task Update(Service service);
}
