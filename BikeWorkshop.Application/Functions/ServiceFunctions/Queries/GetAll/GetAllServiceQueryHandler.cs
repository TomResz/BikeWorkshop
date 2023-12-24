using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Functions.DTO.Enums;
using BikeWorkshop.Application.Interfaces.Repositories;
using MediatR;

namespace BikeWorkshop.Application.Functions.ServiceFunctions.Queries.GetAll;

internal sealed class GetAllServiceQueryHandler
	: IRequestHandler<GetAllServicesQuery, List<ServiceDto>>
{
	private readonly IServiceRepository _serviceRepository;

	public GetAllServiceQueryHandler(IServiceRepository serviceRepository)
	{
		_serviceRepository = serviceRepository;
	}

	public async Task<List<ServiceDto>> Handle(GetAllServicesQuery request, CancellationToken cancellationToken)
	{
		var orders = await _serviceRepository.GetAll();

		var sortedOrders = request.SortingDirection switch
		{
			SortingDirection.Ascending => orders.OrderBy(x => x.Name).ToList(),
			SortingDirection.Descending => orders.OrderByDescending(x => x.Name).ToList(),
			_ => orders
		};
		return ServiceDto.TranslateOrdersToDtos(sortedOrders);
	}
}
