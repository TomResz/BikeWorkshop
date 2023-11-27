using MediatR;

namespace BikeWorkshop.Application.Functions.EmployeeFunctions.Commands.UpdatePassword;

public record UpdatePasswordCommand(
	string Password,
	string ConfirmedPassword) : IRequest;
