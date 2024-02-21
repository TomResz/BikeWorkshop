using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Enums;
using BikeWorkshop.Shared.Exceptions;
using MediatR;

namespace BikeWorkshop.Application.Functions.SummaryFunctions.Command.Delete;
internal class DeleteSummaryCommandHandler
	: IRequestHandler<DeleteSummaryCommand>
{
	private readonly ISummaryRepository _summaryRepository;

	public DeleteSummaryCommandHandler(ISummaryRepository summaryRepository)
	{
		_summaryRepository = summaryRepository;
	}

	public async Task Handle(DeleteSummaryCommand request, CancellationToken cancellationToken)
	{
		var summary = await _summaryRepository.GetByOrderId(request.OrderId)
			?? throw new NotFoundException("Unknown order!");

		if (summary.Order.OrderStatusId is (int)Status.Completed)
		{
			throw new BadRequestException("You can't delete summary of completed order!");
		}
		await _summaryRepository.Delete(summary);
	}
}
