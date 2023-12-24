using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Functions.DTO.Enums;
using BikeWorkshop.Application.Functions.ServiceFunctions.Commands.Add;
using BikeWorkshop.Application.Functions.ServiceFunctions.Commands.Delete;
using BikeWorkshop.Application.Functions.ServiceFunctions.Commands.Update;
using BikeWorkshop.Application.Functions.ServiceFunctions.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BikeWorkshop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ServiceController : ControllerBase
{
	private readonly IMediator _mediator;

	public ServiceController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost("add")]
	public async Task<ActionResult<AddServiceResponse>> Add(AddServiceCommand command)
	{
		var response = await _mediator.Send(command);
		return response.IsAdded ? Ok(response) : Conflict(response);
	}

	[HttpGet("get_all/ascending")]
	public async Task<ActionResult<List<ServiceDto>>> GetAllSortedAscending()
	{
		var response = await _mediator.Send(new GetAllServicesQuery(SortingDirection.Ascending));
		return Ok(response);
	}

	[HttpGet("get_all/descending")]
	public async Task<ActionResult<List<ServiceDto>>> GetAllSortedDescending()
	{
		var response = await _mediator.Send(new GetAllServicesQuery(SortingDirection.Descending));
		return Ok(response);
	}
	[HttpGet("get_all")]
	public async Task<ActionResult<List<ServiceDto>>> GetAll()
	{
		var response = await _mediator.Send(new GetAllServicesQuery());
		return Ok(response);
	}

	[HttpDelete("delete")]
	public async Task<ActionResult> Delete(DeleteServiceCommand command)
	{
		await _mediator.Send(command);
		return NoContent();
	}
	[HttpPut("update")]
	public async Task<ActionResult> Update(UpdateServiceCommand command)
	{
		await _mediator.Send(command);
		return NoContent() ;
	}
}
