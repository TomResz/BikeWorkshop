using MediatR;

namespace BikeWorkshop.Application.Functions.EmployeeFunctions.Commands.Register;

public record RegisterEmployeeCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string ConfirmedPassword,
    int RoleId) : IRequest;
