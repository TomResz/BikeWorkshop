using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Shared.Exceptions;
using MediatR;

namespace BikeWorkshop.Application.Functions.ServiceFunctions.Commands.Update;

internal sealed class UpdateServiceCommandHandler
	: IRequestHandler<UpdateServiceCommand>
{
	private readonly IServiceRepository _repository;

	public UpdateServiceCommandHandler(IServiceRepository repository)
	{
		_repository = repository;
	}

	public async Task Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
	{
		var service = await _repository.GetById(request.ServiceId);
		if (service is null) 
		{ 
			throw new NotFoundException(nameof(service));
		}
		var serviceWithUniqueName = await _repository.GetByName(request.Name);
        if (serviceWithUniqueName is not null)
        {
			throw new BadRequestException("This name already exists!");
        }
		service.Name = request.Name;
		await _repository.Update(service);
    }
}
