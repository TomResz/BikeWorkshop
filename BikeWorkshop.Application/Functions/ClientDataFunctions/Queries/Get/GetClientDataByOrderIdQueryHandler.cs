using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Shared.Exceptions;
using MediatR;

namespace BikeWorkshop.Application.Functions.ClientDataFunctions.Queries.Get;
internal class GetClientDataByOrderIdQueryHandler
    : IRequestHandler<GetClientDataByOrderIdQuery, ClientDataDto>
{
    private readonly IClientDataRepository _clientDataRepository;

    public GetClientDataByOrderIdQueryHandler(IClientDataRepository clientDataRepository)
    {
        _clientDataRepository = clientDataRepository;
    }

    public async Task<ClientDataDto> Handle(GetClientDataByOrderIdQuery request, CancellationToken cancellationToken)
    {
        ClientData clientData = await _clientDataRepository.GeByOrderId(request.OrderId)
            ?? throw new NotFoundException("Client data not found!");
        return new() 
        { 
            PhoneNumber = clientData.PhoneNumber, 
            Email = clientData.Email 
        };
    }
}
