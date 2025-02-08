using MediatR;

namespace BikeWorkshop.Application.Functions.EmployeeFunctions.Commands.Revoke;
public sealed record RevokeTokenCommand(
    string RefreshToken) : IRequest;
