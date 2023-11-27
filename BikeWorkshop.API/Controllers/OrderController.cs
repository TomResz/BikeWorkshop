using BikeWorkshop.Application.Functions.OrderFunctions.Command.CreateOrder;
using BikeWorkshop.Application.Functions.OrderFunctions.Events.CreateOrder;
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

	[HttpPost]
	public async Task<ActionResult> Create(CreateOrderEvent @event)
	{
		await _mediator.Send(@event);
		return Ok();
	}
}
