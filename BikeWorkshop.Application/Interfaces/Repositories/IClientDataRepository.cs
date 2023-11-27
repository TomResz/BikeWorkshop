using BikeWorkshop.Domain.Entities;

namespace BikeWorkshop.Application.Interfaces.Repositories;

public interface IClientDataRepository
{
	Task<ClientData?> GetByPhoneNumberOrEmail(string phoneNumber,string? email);
	Task Add(ClientData data);
}
