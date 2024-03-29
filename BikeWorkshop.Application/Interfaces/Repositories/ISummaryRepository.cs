﻿using BikeWorkshop.Domain.Entities;

namespace BikeWorkshop.Application.Interfaces.Repositories;
public interface ISummaryRepository
{
	Task Add(Summary summary);
	Task Delete(Summary summary);
	Task<Summary?> GetByOrderId(Guid orderId);
	Task Update(Summary summary);
	Task<Summary?> GetByShortUniquerId(string shortId);
}
