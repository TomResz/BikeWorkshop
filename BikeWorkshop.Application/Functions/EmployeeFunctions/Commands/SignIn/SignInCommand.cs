using MediatR;

namespace BikeWorkshop.Application.Functions.EmployeeFunctions.Commands.SignIn;

public sealed record SignInCommand(
	string Email,
	string Password) : IRequest<SignInResponse>;
