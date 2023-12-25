﻿using BikeWorkshop.Application.Fluent_Validation_Extensions;
using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Domain.Entities;
using BikeWorkshop.Shared.Exceptions;
using FluentValidation;
using MediatR;

namespace BikeWorkshop.Application.Functions.ServiceFunctions.Commands.Add;

internal sealed class AddServiceCommandHandler
	: IRequestHandler<AddServiceCommand, AddServiceResponse>
{
	private readonly IServiceRepository _serviceRepository;
	private readonly IValidator<AddServiceCommand> _validator;
	public AddServiceCommandHandler(IServiceRepository serviceRepository, IValidator<AddServiceCommand> validator)
	{
		_serviceRepository = serviceRepository;
		_validator = validator;
	}

	public async Task<AddServiceResponse> Handle(AddServiceCommand request, CancellationToken cancellationToken)
	{
		var resultOfVal = _validator.Validate(request);
		if(!resultOfVal.IsValid) 
		{
			throw new BadRequestException(resultOfVal.Errors.ToJsonString());
		}
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