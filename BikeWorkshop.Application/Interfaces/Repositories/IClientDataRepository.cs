﻿using BikeWorkshop.Domain.Entities;

namespace BikeWorkshop.Application.Interfaces.Repositories;

public interface IClientDataRepository
{
	Task<ClientData?> GetByPhoneNumberOrEmail(string phoneNumber,string? email);
	Task Add(ClientData data);
	Task<string?> GetEmailByOrderId(Guid orderId);
	Task<ClientData?> GeByOrderId(Guid orderId);
	Task<ClientData?> GetByShortId(string shortId);
}
