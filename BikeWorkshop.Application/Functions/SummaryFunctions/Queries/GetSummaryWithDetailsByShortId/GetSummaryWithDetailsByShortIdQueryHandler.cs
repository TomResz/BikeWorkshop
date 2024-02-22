using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Shared.Exceptions;
using MediatR;

namespace BikeWorkshop.Application.Functions.SummaryFunctions.Queries.GetSummaryWithDetailsByShortId;
internal sealed class GetSummaryWithDetailsByShortIdQueryHandler
	: IRequestHandler<GetSummaryWithDetailsByShortIdQuery, SummaryWithDetailsDto>
{
	private readonly ISummaryRepository _summaryRepository;
	private readonly IServiceToOrderRepository _serviceToOrderRepository;
	public GetSummaryWithDetailsByShortIdQueryHandler(ISummaryRepository summaryRepository, IServiceToOrderRepository serviceToOrderRepository)
	{
		_summaryRepository = summaryRepository;
		_serviceToOrderRepository = serviceToOrderRepository;
	}

	public async Task<SummaryWithDetailsDto> Handle(GetSummaryWithDetailsByShortIdQuery request, CancellationToken cancellationToken)
	{
		var order = await _summaryRepository.GetByShortUniquerId(request.ShortId)
				?? throw new NotFoundException("Unknown summary!");
		var services = await _serviceToOrderRepository.GetServiceDetailsByShortId(request.ShortId);
		return SummaryWithDetailsDto.Translate(order, services);
	}
}
