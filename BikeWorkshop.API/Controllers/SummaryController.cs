using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Functions.SummaryFunctions.Command.CreateSummaryForOrder;
using BikeWorkshop.Application.Functions.SummaryFunctions.Command.Delete;
using BikeWorkshop.Application.Functions.SummaryFunctions.Queries.GetSummaryWithDetails;
using BikeWorkshop.Application.Functions.SummaryFunctions.Queries.GetSummaryWithDetailsByShortId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BikeWorkshop.API.Controllers;
[Route("api/[controller]")]
[Authorize(Roles = "Manager,Worker")]
[ApiController]
[SwaggerResponse(StatusCodes.Status401Unauthorized)]
public class SummaryController : ControllerBase
{
	private readonly IMediator _mediator;

	public SummaryController(IMediator mediator)
	{
		_mediator = mediator;
	}
	/// <summary>
	/// Returns summary with details.
	/// </summary>
	/// <param name="orderId">Order Id (<b>Guid</b>).</param>
	/// <returns>Summary with details</returns>
	[SwaggerResponse(StatusCodes.Status200OK)]
	[SwaggerResponse(StatusCodes.Status404NotFound)]
	[HttpGet("{orderId::guid}")]
	public async Task<ActionResult<SummaryWithDetailsDto>> Get(Guid orderId)
	{
		var query = new GetSummaryWithDetailsQuery(orderId);
		return Ok(await _mediator.Send(query));
	}
	/// <summary>
	/// Returns summary with extra details.
	/// </summary>
	/// <param name="shortId">Short unique Id.</param>
	/// <returns>Summary with details.</returns>
	[SwaggerResponse(StatusCodes.Status200OK)]
	[SwaggerResponse(StatusCodes.Status404NotFound,"If summary not found.")]
	[HttpGet("short/{shortId}")]
	public async Task<ActionResult<SummaryWithDetailsDto>> GetByShortId([FromRoute]string shortId)
	{
		var query = new GetSummaryWithDetailsByShortIdQuery(shortId);
		return Ok(await _mediator.Send(query));
	}

	/// <summary>
	/// Creates summary to order.
	/// </summary>
	/// <param name="command"> Command includes order Id and conclusions (optional field).</param>
	/// <returns></returns>
	[SwaggerResponse(StatusCodes.Status201Created, "Successfully created summary.")]
	[SwaggerResponse(StatusCodes.Status404NotFound, "If the order is unknown.")]
	[SwaggerResponse(StatusCodes.Status400BadRequest, "If data is invalid.")]
	[HttpPost("create")]
	public async Task<IActionResult> CreateSummary(CreateSummaryForOrderCommand command)
	{
		await _mediator.Send(command);
		return Created("api/Summary/create",null);
	}

    /// <summary>
    /// Delete summary of order with only completed status and changes status to original.
    /// </summary>
    /// <param name="orderId">Order Id.</param>
    [SwaggerResponse(StatusCodes.Status204NoContent, "Successfully removed.")]
	[SwaggerResponse(StatusCodes.Status404NotFound, "If summary with given order id was not found.")]
	[SwaggerResponse(StatusCodes.Status400BadRequest, "If the order has already been received.")]
	[HttpDelete("delete/{orderId:guid}")]
	public async Task<IActionResult> Delete(Guid orderId)
	{
		await _mediator.Send(new DeleteSummaryCommand(orderId));
		return NoContent();
	}
}
