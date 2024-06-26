﻿using BikeWorkshop.API.QueryPoliticy;
using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Functions.DTO.Enums;
using BikeWorkshop.Application.Functions.OrderFunctions.Command.RetrieveOrder;
using BikeWorkshop.Application.Functions.OrderFunctions.Events.CreateOrder;
using BikeWorkshop.Application.Functions.OrderFunctions.Queries.GePageOfCompleted;
using BikeWorkshop.Application.Functions.OrderFunctions.Queries.GetActual;
using BikeWorkshop.Application.Functions.OrderFunctions.Queries.GetCompleted;
using BikeWorkshop.Application.Functions.OrderFunctions.Queries.GetOrderHistoryByShortUniqueId;
using BikeWorkshop.Application.Functions.OrderFunctions.Queries.GetPageOfCurrents;
using BikeWorkshop.Application.Functions.OrderFunctions.Queries.GetPageOfRetrieved;
using BikeWorkshop.Application.Functions.OrderFunctions.Queries.GetRetrieved;
using BikeWorkshop.Application.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BikeWorkshop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[SwaggerResponse(StatusCodes.Status401Unauthorized)]
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
	/// <response code="201">If the order creation is successful.</response>
	/// <response code="400">If the order data is invalid.</response>
	[HttpPost("create")]
	[Authorize(Roles = "Manager,Worker")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[SwaggerResponse(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult> Create(CreateOrderEvent @event)
	{
		await _mediator.Send(@event);
		return Created("api/order/create",null);
	}


	/// <summary>
	/// Returns list of active orders.
	/// </summary>
	/// <param name="direction">
	/// Sorting direction:<br></br>
	/// <b>asc</b>-Ascending
	/// <br><b>desc</b>-Descending</br>
	/// </param>
	/// <returns>List of current orders.</returns>
	/// <response code="200"> If sorting direction is correct.
	/// </response>
	/// <response code="400"> If sorting direction is invalid.
	/// </response>
	[HttpGet("current/{direction}")]
	[Authorize(Roles = "Manager,Worker")]
	[ProducesResponseType(typeof(List<OrderDto>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<List<OrderDto>>> GetAllCurrent([FromRoute] string direction)
	{
		var query = new GetCurrentOrdersQuery(SortingParameters.FromString(direction));
		var orders = await _mediator.Send(query);
		return Ok(orders);
	}


	/// <summary>
	/// Returns list of completed orders.
	/// </summary>
	/// <param name="direction">
	/// Sorting direction:<br></br>
	/// <b>asc</b>-Ascending
	/// <br><b>desc</b>-Descending</br>
	/// </param>
	/// <returns>List of completed orders.</returns>
	/// <response code="200"> If sorting direction is correct.
	/// </response>
	/// <response code="400"> If sorting direction is invalid.
	/// </response>
	[HttpGet("completed/{direction}")]
	[Authorize(Roles = "Manager,Worker")]
	[ProducesResponseType(typeof(List<OrderDto>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<List<OrderDto>>> GetAllCompleted([FromRoute] string direction)
	{
		var query = new GetCompletedOrderQuery(SortingParameters.FromString(direction));
		var orders = await _mediator.Send(query);
		return Ok(orders);
	}

	/// <summary>
	/// Returns list of retrieved orders.
	/// </summary>
	/// <param name="direction">
	/// Sorting direction:<br></br>
	/// <b>asc</b>-Ascending
	/// <br><b>desc</b>-Descending</br>
	/// </param>
	[HttpGet("retrieved/{direction}")]
	[Authorize(Roles = "Manager,Worker")]
	[ProducesResponseType(typeof(List<OrderDto>), StatusCodes.Status200OK)]
	[SwaggerResponse(StatusCodes.Status400BadRequest,"Invalid sorting parameters.")]
	public async Task<ActionResult<List<OrderDto>>> GetAllRetrieved([FromRoute] string direction)
	{
		var query = new GetRetrievedOrdersQuery(SortingParameters.FromString(direction));
		var orders = await _mediator.Send(query);
		return Ok(orders);
	}


	/// <summary>
	/// Returns page of current orders.
	/// </summary>
	[HttpGet("current")]
	[Authorize(Roles = "Manager,Worker")]
	[ProducesResponseType(typeof(PagedList<OrderDto>), StatusCodes.Status200OK)]
	public async Task<ActionResult<PagedList<OrderDto>>> GetCurrentPage([FromQuery] PageParameters parameters)
	{
		var query = new GetPageOfCurrentsOrdersQuery(parameters.Page, parameters.PageSize);
		var response = await _mediator.Send(query);
		return Ok(response);
	}

	/// <summary>
	/// Used to confirm receipt of an order.
	/// </summary>
	/// <param name="command">The command contains the Id (Guid) of the order.</param>
	[SwaggerResponse(StatusCodes.Status204NoContent)]
	[SwaggerResponse(StatusCodes.Status400BadRequest, "If trying to delete summary of a received order.")]
	[SwaggerResponse(StatusCodes.Status404NotFound, "If there is no given order with a given Id.")]
	[HttpPatch("retrieve-order")]
	[Authorize(Roles = "Manager,Worker")]
	public async Task<IActionResult> RetrieveOrder(RetrieveOrderCommand command)
	{
		await _mediator.Send(command);
		return NoContent();
	}

	/// <summary>
	/// Returns history of order to user.
	/// </summary>
	/// <param name="shortId">Short unique Id.</param>
	/// <returns>History of order.</returns>
	[SwaggerResponse( StatusCodes.Status200OK, "Retrieves a history of data.",typeof(OrderHistoryDto))]
	[SwaggerResponse(StatusCodes.Status404NotFound, "If there is no given order with a given short Id.")]
	[HttpGet("search/{shortId}")]
	public async Task<ActionResult<OrderHistoryDto>> GetByShortId(string shortId)
	{
		var orderHistory = await _mediator.Send(new GetOrderHistoryByShortUniqueIdQuery(shortId));
		return Ok(orderHistory);
	}

	/// <summary>
	/// Returns page of completed orders.
	/// </summary>
	/// <param name="parameters">Includes page number and size.</param>
	/// <param name="direction">Includes order sorting direction. Optional field.</param>
	/// <returns>Page of completed orders.</returns>
	[ProducesResponseType(typeof(PagedList<OrderDto>),StatusCodes.Status200OK)]
	[SwaggerResponse(StatusCodes.Status400BadRequest,"Invalid page parameters or direction.")]
	[HttpGet("completed")]
	[Authorize(Roles = "Manager,Worker")]
	public async Task<ActionResult<PagedList<OrderDto>>> GetPageOfCompleted([FromQuery] PageParameters parameters, [FromQuery] string? direction)
	{
		SortingDirection? sortingDirection = null;
		if (direction is not null)
		{
			sortingDirection = SortingParameters.FromString(direction);
		}
		var orders = await _mediator.Send(new GetPageOfCompletedQuery(parameters.Page,parameters.PageSize,sortingDirection));
		return Ok(orders);
	}
	/// <summary>
	/// Returns page of retrieved orders.
	/// </summary>
	/// <param name="parameters">Includes page number and size.</param>
	/// <param name="direction">Includes order sorting direction. Optional field.</param>
	/// <returns>Page of retrieved orders.</returns>
	[ProducesResponseType(typeof(PagedList<OrderDto>), StatusCodes.Status200OK)]
	[SwaggerResponse(StatusCodes.Status400BadRequest,"Invalid page parameters or sorting direction.")]
	[HttpGet("retrieved")]
	[Authorize(Roles = "Manager,Worker")]
	public async Task<ActionResult<PagedList<OrderDto>>> GetPageOfRetrieved([FromQuery] PageParameters parameters, [FromQuery] string? direction)
	{
		SortingDirection? sortingDirection = null;
		if (direction is not null)
		{
			sortingDirection = SortingParameters.FromString(direction);
		}
		var orders = await _mediator.Send(new GetPageOfRetrievedOrdersQuery(parameters.Page, parameters.PageSize, sortingDirection));
		return Ok(orders);
	}
}
