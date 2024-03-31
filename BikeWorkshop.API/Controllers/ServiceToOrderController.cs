using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Functions.ServiceToOrderFunctions.Commands.Add;
using BikeWorkshop.Application.Functions.ServiceToOrderFunctions.Commands.Delete;
using BikeWorkshop.Application.Functions.ServiceToOrderFunctions.Queries.GetByOrder;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BikeWorkshop.API.Controllers;

[Route("api/[controller]")]
[Authorize(Roles = "Manager,Worker")]
[ApiController]
public class ServiceToOrderController : ControllerBase
{
	private readonly IMediator _mediator;

	public ServiceToOrderController(IMediator mediator)
	{
		_mediator = mediator;
	}
	/// <summary>
	/// Connects service with order with service's count and price.
	/// </summary>
	/// <param name="command">The command include:
	/// <br></br>-Service ID (<b>GUID</b>),
	/// <br></br>-Order ID (<b>GUID</b>),
	/// <br></br>-Service count (<b>int</b>),
	/// <br></br>-Price (<b>decimal</b>).
	/// </param>
	[HttpPost("add")]
	[SwaggerResponse(StatusCodes.Status201Created)]
	[SwaggerResponse(StatusCodes.Status404NotFound,"Order or service with given Id not found.")]
	[SwaggerResponse(StatusCodes.Status400BadRequest, "Order has already been completed.")]
	public async Task<IActionResult> Add(AddServiceToOrderCommand command)
	{
		await _mediator.Send(command);
		return Created("api/servicetoorder/add",null);
	}

    /// <summary>
    /// Delete entity.
    /// </summary>
    /// <param name="serviceToOrderId">Include serviceToOrder entity unique ID.</param>
    [HttpDelete("delete/{serviceToOrderId:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[SwaggerResponse(StatusCodes.Status404NotFound,"Unknown service to order Id.")]
	public async Task<IActionResult> Delete(Guid serviceToOrderId)
	{
		await _mediator.Send(new DeleteServiceToOrderCommand(serviceToOrderId));
		return NoContent();
	}

	/// <summary>
	/// Fetch list of service of orders.
	/// </summary>
	/// <param name="orderId">Order unique ID</param>
	/// <returns>List of serviceToOrderDto's.</returns>
	[HttpGet("{orderId:guid}/all")]
	[ProducesResponseType(typeof(List<ServiceToOrderDto>),StatusCodes.Status200OK)]
	[SwaggerResponse(StatusCodes.Status404NotFound,"Unknown order Id.")]
	public async Task<ActionResult<List<ServiceToOrderDto>>> GetByOrderId([FromRoute]Guid orderId)
	{
		var query = new GetServiceToOrderByOrderQuery(orderId);
		var response = await _mediator.Send(query);
		return Ok(response);
	}
}
