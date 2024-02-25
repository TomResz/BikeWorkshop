using BikeWorkshop.Domain.Entities;
using MediatR;

namespace BikeWorkshop.Application.Functions.ClientDataFunctions.Commands.CreateClientData;

public record CreateClientDataCommand(
    string? Email,
    string PhoneNumber) : IRequest<ClientData>;
