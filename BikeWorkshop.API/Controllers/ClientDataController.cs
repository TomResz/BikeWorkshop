using BikeWorkshop.Application.Functions.ClientDataFunctions.Queries.Get;
using BikeWorkshop.Application.Functions.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikeWorkshop.API.Controllers;
[Route("api/[controller]")]
[Authorize(Roles ="Worker,Manager")]
[ApiController]
public class ClientDataController : ControllerBase
{
	private readonly IMediator _mediator;

	public ClientDataController(IMediator mediator)
	{
		_mediator = mediator;
	}
	/// <summary>
	/// Fetch basic client data.
	/// </summary>
	/// <param name="orderId">Order unique Id.</param>
	/// <returns>Client's phone number and email.</returns>
	[HttpGet("get/{orderId::guid}")]
	public async Task<ActionResult<ClientDataDto>> GetByOrderId(Guid orderId)
	{
		var query = new GetClientDataByOrderIdQuery(orderId);
		var response = await _mediator.Send(query);
		return Ok(response);
	}


}
