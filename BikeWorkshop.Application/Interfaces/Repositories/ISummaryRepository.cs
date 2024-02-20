using BikeWorkshop.Domain.Entities;

namespace BikeWorkshop.Application.Interfaces.Repositories;
public interface ISummaryRepository
{
	Task Add(Summary summary);
	Task<Summary?> GetByOrderId(Guid orderId);
	Task Update(Summary summary);
}
