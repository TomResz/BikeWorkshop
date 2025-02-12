﻿using BikeWorkshop.Domain.Entities;

namespace BikeWorkshop.Application.Interfaces.Repositories;

public interface IEmployeeRepository
{
	Task Register(Employee employee);
	Task<Employee?> GetByEmail(string email);
	Task Update(Employee employee);
	Task<Employee?> GetById(Guid id);
	Task<List<Employee>> GetAll();
    Task<Employee?> GetByIdAndRefreshTokenAsync(Guid id, string refreshToken);
}
