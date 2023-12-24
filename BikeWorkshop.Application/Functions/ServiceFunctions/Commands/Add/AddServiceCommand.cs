using MediatR;

namespace BikeWorkshop.Application.Functions.ServiceFunctions.Commands.Add;

public record AddServiceCommand(
	string Name) : IRequest<AddServiceResponse>;
