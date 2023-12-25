using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Functions.DTO.Enums;
using BikeWorkshop.Application.Functions.OrderFunctions.Events.CreateOrder;
using BikeWorkshop.Application.Functions.OrderFunctions.Queries.GetActual;
using BikeWorkshop.Application.Functions.OrderFunctions.Queries.GetCompleted;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BikeWorkshop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
	private readonly IMediator _mediator;
	public OrderController(IMediator mediator)
	{
		_mediator = mediator;
	}
	/// <summary>
	/// Creates a new order based on the provided order creation event.
	/// </summary>
	/// <param name="event">The order creation event containing necessary information.</param>
	/// <returns>An ActionResult representing the result of the order creation operation.</returns>
	/// <response code="200">If the order creation is successful.</response>
	/// <response code="400">If the order data is invalid.</response>
	[HttpPost("create_order")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Create(CreateOrderEvent @event)
	{
		await _mediator.Send(@event);
		return Ok();
	}


	/// <summary>
	/// Returns list of active orders.
	/// </summary>
	/// <param name="direction">
	/// Sorting direction:<br></br>
	/// <b>1</b>-Ascending
	/// <br><b>2</b>-Descending</br>
	/// </param>
	/// <returns>List of current orders.</returns>
	/// <response code="200"> If sorting direction is correct.
	/// </response>
	/// <response code="400"> If sorting direction is invalid.
	/// </response>
	[HttpGet("get_active/{direction:int}")]
	[ProducesResponseType(typeof(List<OrderDto>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<List<OrderDto>>> GetAllCurrent([FromRoute]int direction)
	{
		if (!Enum.IsDefined(typeof(SortingDirection), direction))
		{
			return BadRequest("Invalid direction ID!");
		}
		var query = new GetCurrentOrdersQuery((SortingDirection)direction);
		var orders = await _mediator.Send(query);
		return Ok(orders);
	}


	/// <summary>
	/// Returns list of completed orders.
	/// </summary>
	/// <param name="direction">
	/// Sorting direction:<br></br>
	/// <b>1</b>-Ascending
	/// <br><b>2</b>-Descending</br>
	/// </param>
	/// <returns>List of completed orders.</returns>
	/// <response code="200"> If sorting direction is correct.
	/// </response>
	/// <response code="400"> If sorting direction is invalid.
	/// </response>
	[HttpGet("get_completed/{direction:int}")]
	[ProducesResponseType(typeof(List<OrderDto>),StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(string),StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<List<OrderDto>>> GetAllCompleted([FromRoute] int direction)
	{
		if (!Enum.IsDefined(typeof(SortingDirection),direction))
		{
			return BadRequest("Invalid direction ID!");
		}
		var query = new GetCompletedOrderQuery((SortingDirection)direction);
		var orders = await _mediator.Send(query);
		return Ok(orders);
	}
}
