using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Shared.Exceptions;
using MediatR;

namespace BikeWorkshop.Application.Functions.ServiceToOrderFunctions.Commands.Delete;

internal sealed class DeleteServiceToOrderCommandHandler
	: IRequestHandler<DeleteServiceToOrderCommand>
{
	private readonly IServiceToOrderRepository _repository;

	public DeleteServiceToOrderCommandHandler(IServiceToOrderRepository repository)
	{
		_repository = repository;
	}

	public async Task Handle(DeleteServiceToOrderCommand request, CancellationToken cancellationToken)
	{
		var obj = await _repository.GetById(request.ServiceToOrderId);
		if(obj is null)
		{
			throw new NotFoundException($"Object with {request.ServiceToOrderId} was not found!");
		}
		await _repository.Delete(obj);
	}
}
