using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Functions.ServiceToOrderFunctions.Commands.Add;
using BikeWorkshop.Application.Functions.ServiceToOrderFunctions.Commands.Delete;
using BikeWorkshop.Application.Functions.ServiceToOrderFunctions.Queries.GetByOrder;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Add(AddServiceToOrderCommand command)
	{
		await _mediator.Send(command);
		return Ok();
	}

	/// <summary>
	/// Delete entity.
	/// </summary>
	/// <param name="command">Include serviceToOrder entity unique ID.</param>
	[HttpDelete("delete")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Delete(DeleteServiceToOrderCommand command)
	{
		await _mediator.Send(command);
		return NoContent();
	}

	/// <summary>
	/// Fetch list of service of orders.
	/// </summary>
	/// <param name="orderId">Order unique ID</param>
	/// <returns>List of serviceToOrderDto's.</returns>
	[HttpGet("{orderId:guid}/all")]
	[ProducesResponseType(typeof(List<ServiceToOrderDto>),StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<List<ServiceToOrderDto>>> GetByOrderId([FromRoute]Guid orderId)
	{
		var query = new GetServiceToOrderByOrderQuery(orderId);
		var response = await _mediator.Send(query);
		return Ok(response);
	}
}
