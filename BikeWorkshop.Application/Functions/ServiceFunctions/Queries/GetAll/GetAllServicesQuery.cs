using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Functions.DTO.Enums;
using MediatR;

namespace BikeWorkshop.Application.Functions.ServiceFunctions.Queries.GetAll;

public record GetAllServicesQuery(SortingDirection? SortingDirection=null) : IRequest<List<ServiceDto>>;
