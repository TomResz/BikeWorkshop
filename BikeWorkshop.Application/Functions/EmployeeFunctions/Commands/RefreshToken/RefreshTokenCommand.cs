using BikeWorkshop.Application.Functions.DTO;
using MediatR;

namespace BikeWorkshop.Application.Functions.EmployeeFunctions.Commands.RefreshToken;
public sealed record RefreshTokenCommand(
    string AccessToken,
    string RefreshToken) : IRequest<JwtDto>;
