using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Shared.Exceptions;
using MediatR;

namespace BikeWorkshop.Application.Functions.SummaryFunctions.Queries.GetSummaryWithDetails;
internal class GetSummaryWithDetailsQueryHandler
	: IRequestHandler<GetSummaryWithDetailsQuery, SummaryWithDetailsDto>
{
	private readonly ISummaryRepository _summaryRepository;
	private readonly IServiceToOrderRepository _serviceToOrderRepository;
	public GetSummaryWithDetailsQueryHandler(
		IServiceToOrderRepository serviceToOrderRepository, ISummaryRepository summaryRepository)
	{
		_serviceToOrderRepository = serviceToOrderRepository;
		_summaryRepository = summaryRepository;
	}

	public async Task<SummaryWithDetailsDto> Handle(GetSummaryWithDetailsQuery request, CancellationToken cancellationToken)
	{
		var order = await _summaryRepository.GetByOrderId(request.OrderId)
			?? throw new NotFoundException("Unknown summary!");
		var services = await _serviceToOrderRepository.GetServiceDetailsByOrderId(request.OrderId);
		return SummaryWithDetailsDto.Translate(order,services);
	}
}
