using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Shared.Exceptions;
using MediatR;

namespace BikeWorkshop.Application.Functions.ServiceFunctions.Commands.Delete;

internal sealed class DeleteServiceCommandHandler
	: IRequestHandler<DeleteServiceCommand>
{
	private readonly IServiceRepository _repository;

	public DeleteServiceCommandHandler(IServiceRepository repository)
	{
		_repository = repository;
	}

	public async Task Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
	{
		var service = await _repository.GetById(request.ServiceId);
		if(service is null)
		{
			throw new NotFoundException("Service not found!");
		}
		if(service.ServiceToOrders.Count is not 0)
		{
			throw new BadRequestException("You can't remove this service because it has orders!");
		}
		await _repository.Delete(service);
	}
}
