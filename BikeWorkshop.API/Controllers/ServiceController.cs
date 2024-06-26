﻿using BikeWorkshop.API.QueryPoliticy;
using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Functions.ServiceFunctions.Commands.Add;
using BikeWorkshop.Application.Functions.ServiceFunctions.Commands.Delete;
using BikeWorkshop.Application.Functions.ServiceFunctions.Commands.Update;
using BikeWorkshop.Application.Functions.ServiceFunctions.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BikeWorkshop.API.Controllers;

[Route("api/[controller]")]
[Authorize(Roles = "Manager,Worker")]
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
	/// <response code="409"> If service currently exists.
	/// </response>
	[HttpPost("add")]
	[ProducesResponseType(typeof(AddServiceResponse),StatusCodes.Status201Created)]
	[SwaggerResponse(StatusCodes.Status400BadRequest,"Invalid service name.")]
	[ProducesResponseType(typeof(AddServiceResponse),StatusCodes.Status409Conflict)]
	public async Task<ActionResult<AddServiceResponse>> Add(AddServiceCommand command)
	{
		var response = await _mediator.Send(command);
		return response.IsAdded ? Created("api/order/add",null) : Conflict(response);
	}


	/// <summary>
	/// Retrieves a sorted list of services.
	/// </summary>
	/// <param name="direction">
	/// Sorting direction:<br></br>
	/// <b>asc</b>-Ascending
	/// <br><b>desc</b>-Descending</br>
	/// </param>
	/// <returns>List of current orders.</returns>
	/// <response code="200"> If sorting direction is correct.
	/// </response>
	/// <response code="400"> If sorting direction is invalid.
	/// </response>
	/// <returns>Sorted list of services.</returns>
	[HttpGet("all")]
	[ProducesResponseType(typeof(List<ServiceDto>),StatusCodes.Status200OK)]
	public async Task<ActionResult<List<ServiceDto>>> GetAll([FromQuery]string? direction)
	{
		var response = await _mediator.Send(new GetAllServicesQuery(direction is not null 
			? SortingParameters.FromString(direction) 
			: null));
		return Ok(response);
	}

    /// <summary>
    /// Deletes a service based on the provided command.
    /// </summary>
    /// <param name="serviceId">Service Id</param>
    /// <returns>
    /// </returns>
    /// <response code="204">If service is successfully deleted.</response>
    /// <response code="400">If service is linked with other orders.</response>
    /// <response code="404">If service with provided ID doesn't exists.</response>
    [HttpDelete("delete/{serviceId}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(string),StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Delete([FromRoute]Guid serviceId)
	{
		var command = new DeleteServiceCommand(serviceId);
		await _mediator.Send(command);
		return NoContent();
	}

	/// <summary>
	/// Update a service based on the provided command.
	/// </summary>
	/// <param name="command">The command specifying service UID and new name.</param>
	/// <returns>
	/// </returns>
	/// <response code="204">If service was successfully deleted.</response>
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
