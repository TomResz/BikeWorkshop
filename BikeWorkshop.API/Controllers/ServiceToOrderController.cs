using BikeWorkshop.Application.Functions.ServiceToOrderFunctions.Commands.Add;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BikeWorkshop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ServiceToOrderController : ControllerBase
{
	private readonly IMediator _mediator;

	public ServiceToOrderController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost("add")]
	public async Task<IActionResult> Add(AddServiceToOrderCommand command)
	{
		await _mediator.Send(command);
		return Ok();
	}
}
