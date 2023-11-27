﻿using BikeWorkshop.Domain.Entities;

namespace BikeWorkshop.Application.Functions.DTO;

public class EmployeeDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string RoleName { get; set; }
}
