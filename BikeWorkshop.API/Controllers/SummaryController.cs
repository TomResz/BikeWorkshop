using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Functions.SummaryFunctions.Command.CreateSummaryForOrder;
using BikeWorkshop.Application.Functions.SummaryFunctions.Command.Delete;
using BikeWorkshop.Application.Functions.SummaryFunctions.Queries.GetSummaryWithDetails;
using BikeWorkshop.Application.Functions.SummaryFunctions.Queries.GetSummaryWithDetailsByShortId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikeWorkshop.API.Controllers;
[Route("api/[controller]")]
[Authorize(Roles = "Manager,Worker")]
[ApiController]
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
	/// <param name="orderId">Order Id(<b>Guid</b>).</param>
	/// <returns>Summary with details</returns>
	[HttpGet("get={orderId::guid}")]
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
	[HttpGet("get/{shortId}")]
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
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[HttpPost("create")]
	public async Task<IActionResult> CreateSummary(CreateSummaryForOrderCommand command)
	{
		await _mediator.Send(command);
		return NoContent();
	}

	/// <summary>
	/// Delete summary of order with only completed status and changes status to original.
	/// </summary>
	/// <param name="command">Includes order Id.</param>
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[HttpDelete("delete")]
	public async Task<IActionResult> Delete(DeleteSummaryCommand command)
	{
		await _mediator.Send(command);
		return NoContent();
	}
}
