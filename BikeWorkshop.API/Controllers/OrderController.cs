using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Functions.DTO.Enums;
using BikeWorkshop.Application.Functions.OrderFunctions.Events.CreateOrder;
using BikeWorkshop.Application.Functions.OrderFunctions.Queries.GetActual;
using BikeWorkshop.Application.Functions.OrderFunctions.Queries.GetCompleted;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
	[HttpPost("create_order")]
	public async Task<ActionResult> Create(CreateOrderEvent @event)
	{
		await _mediator.Send(@event);
		return Ok();
	}

	[HttpGet("get_active/{direction}")]
	public async Task<ActionResult<List<OrderDto>>> GetAllCurrent([FromRoute]int direction)
	{
		if(direction is > 2 or < 1)
		{
			return BadRequest("Invalid direction ID!");
		}
		var query = new GetCurrentOrdersQuery((SortingDirection)direction);
		var orders = await _mediator.Send(query);
		return Ok(orders);
	}
	[HttpGet("get_completed/{direction}")]
	public async Task<ActionResult<List<OrderDto>>> GetAllCompleted([FromRoute] int direction)
	{
		if (direction is > 2 or < 1)
		{
			return BadRequest("Invalid direction ID!");
		}
		var query = new GetCompletedOrderQuery((SortingDirection)direction);
		var orders = await _mediator.Send(query);
		return Ok(orders);
	}
}
