using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using FluentValidation;
using MediatR;

namespace BikeWorkshop.Application.Functions.ServiceFunctions.Commands.Add;

internal sealed class AddServiceCommandHandler
	: IRequestHandler<AddServiceCommand, AddServiceResponse>
{
	private readonly IServiceRepository _serviceRepository;
	public AddServiceCommandHandler(IServiceRepository serviceRepository, IValidator<AddServiceCommand> validator)
	{
		_serviceRepository = serviceRepository;
	}

	public async Task<AddServiceResponse> Handle(AddServiceCommand request, CancellationToken cancellationToken)
	{
		var service = new Service()
		{
			Id = Guid.NewGuid(),
			Name = request.Name,
		};
		var isAdded = await _serviceRepository.Add(service);
		return isAdded ? new AddServiceResponse(isAdded,null)
			: new AddServiceResponse(isAdded, "The service currently exists in the database");
	}
}
