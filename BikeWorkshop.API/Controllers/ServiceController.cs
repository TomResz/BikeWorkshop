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
	/// <summary>
	/// Adds a new service using the provided command.
	/// </summary>
	/// <param name="command">The command containing information to add the service.</param>
	/// <returns>Response with boolean value and feedback message.</returns>
	/// <response code="200"> If service creation is successful.
	/// </response>
	/// <response code="409"> If service currently exists.
	/// </response>
	[HttpPost("add")]
	[ProducesResponseType(typeof(AddServiceResponse),StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(AddServiceResponse),StatusCodes.Status409Conflict)]
	public async Task<ActionResult<AddServiceResponse>> Add(AddServiceCommand command)
	{
		var response = await _mediator.Send(command);
		return response.IsAdded ? Ok(response) : Conflict(response);
	}


	/// <summary>
	/// Retrieves a sorted list of services in ascending order.
	/// </summary>
	/// <returns>Sorted list of services.</returns>
	[HttpGet("get_all/ascending")]
	[ProducesResponseType(typeof(List<ServiceDto>),StatusCodes.Status200OK)]
	public async Task<ActionResult<List<ServiceDto>>> GetAllSortedAscending()
	{
		var response = await _mediator.Send(new GetAllServicesQuery(SortingDirection.Ascending));
		return Ok(response);
	}


	/// <summary>
	/// Retrieves a sorted list of services in descending order.
	/// </summary>
	/// <returns>Sorted list of services.</returns>
	[HttpGet("get_all/descending")]
	[ProducesResponseType(typeof(List<ServiceDto>), StatusCodes.Status200OK)]
	public async Task<ActionResult<List<ServiceDto>>> GetAllSortedDescending()
	{
		var response = await _mediator.Send(new GetAllServicesQuery(SortingDirection.Descending));
		return Ok(response);
	}


	/// <summary>
	/// Retrieves a list of all services.
	/// </summary>
	/// <returns>List of orders.</returns>
	[HttpGet("get_all")]
	public async Task<ActionResult<List<ServiceDto>>> GetAll()
	{
		var response = await _mediator.Send(new GetAllServicesQuery());
		return Ok(response);
	}


	/// <summary>
	/// Deletes a service based on the provided command.
	/// </summary>
	/// <param name="command">The command specifying service UID.</param>
	/// <returns>
	/// </returns>
	/// <response code="204">If service is successfully deleted.</response>
	/// <response code="400">If service is linked with other orders.</response>
	/// <response code="404">If service with provided ID doesn't exists.</response>
	[HttpDelete("delete")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(string),StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Delete(DeleteServiceCommand command)
	{
		await _mediator.Send(command);
		return NoContent();
	}

	/// <summary>
	/// Update a service based on the provided command.
	/// </summary>
	/// <param name="command">The command specifying service UID and new name.</param>
	/// <returns>
	/// </returns>
	/// <response code="204">If service is successfully deleted.</response>
	/// <response code="400">If service with provided ID doesn't exists.</response>
	/// <response code="404">If service with provided name exists or command data is invalid.</response>
	[HttpPut("update")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(string),StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Update(UpdateServiceCommand command)
	{
		await _mediator.Send(command);
		return NoContent();
	}
}
