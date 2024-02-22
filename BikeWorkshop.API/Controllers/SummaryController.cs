using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Functions.SummaryFunctions.Command.CreateSummaryForOrder;
using BikeWorkshop.Application.Functions.SummaryFunctions.Command.Delete;
using BikeWorkshop.Application.Functions.SummaryFunctions.Queries.GetSummaryWithDetails;
using BikeWorkshop.Application.Functions.SummaryFunctions.Queries.GetSummaryWithDetailsByShortId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BikeWorkshop.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SummaryController : ControllerBase
{
	private readonly IMediator _mediator;

	public SummaryController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("get={orderId::guid}")]
	public async Task<ActionResult<SummaryWithDetailsDto>> Get(Guid orderId)
	{
		var query = new GetSummaryWithDetailsQuery(orderId);
		return Ok(await _mediator.Send(query));
	}

	[HttpGet("get/{shortId}")]
	public async Task<ActionResult<SummaryWithDetailsDto>> GetByShortId([FromRoute]string shortId)
	{
		var query = new GetSummaryWithDetailsByShortIdQuery(shortId);
		return Ok(await _mediator.Send(query));
	}

	[HttpPost("create")]
	public async Task<IActionResult> CreateSummary(CreateSummaryForOrderCommand command)
	{
		await _mediator.Send(command);
		return NoContent();
	}

	[HttpDelete("delete")]
	public async Task<IActionResult> Delete(DeleteSummaryCommand command)
	{
		await _mediator.Send(command);
		return NoContent();
	}
}
