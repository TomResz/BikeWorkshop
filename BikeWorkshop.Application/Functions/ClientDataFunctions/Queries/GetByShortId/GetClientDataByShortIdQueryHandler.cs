using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Shared.Exceptions;
using MediatR;

namespace BikeWorkshop.Application.Functions.ClientDataFunctions.Queries.GetByShortId;
internal sealed class GetClientDataByShortIdQueryHandler
	: IRequestHandler<GetClientDataByShortIdQuery, ClientDataDto>
{
	private readonly IClientDataRepository _clientDataRepository;

	public GetClientDataByShortIdQueryHandler(IClientDataRepository clientDataRepository)
	{
		_clientDataRepository = clientDataRepository;
	}

	public async Task<ClientDataDto> Handle(GetClientDataByShortIdQuery request, CancellationToken cancellationToken)
	{
		ClientData data = await _clientDataRepository.GetByShortId(request.ShortId) ??
			throw new NotFoundException("Client data not found!");
		return new() { Email = data.Email , PhoneNumber = data.PhoneNumber};
	}
}
