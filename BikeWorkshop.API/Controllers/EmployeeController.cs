using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Functions.EmployeeFunctions.Commands.Register;
using BikeWorkshop.Application.Functions.EmployeeFunctions.Commands.SignIn;
using BikeWorkshop.Application.Functions.EmployeeFunctions.Commands.UpdatePassword;
using BikeWorkshop.Application.Functions.EmployeeFunctions.Queries.GetEmployees;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BikeWorkshop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[SwaggerResponse(StatusCodes.Status401Unauthorized)]
public class EmployeeController : ControllerBase
{
	private readonly IMediator _mediator;

	public EmployeeController(IMediator mediator)
	{
		_mediator = mediator;
	}

	/// <summary>
	/// Retrieves a list of all employees.
	/// </summary>
	[HttpGet("get_all")]
	[Authorize(Roles = "Manager")]
	[ProducesResponseType(typeof(List<EmployeeDto>), StatusCodes.Status200OK)]
	[SwaggerResponse(StatusCodes.Status200OK,"Fetch a list of employees.",typeof(List<EmployeeDto>))]
	[SwaggerResponse(StatusCodes.Status401Unauthorized)]
	[SwaggerResponse(StatusCodes.Status403Forbidden,"Only manager can fetch data from this endpoint.")]

	public async Task<ActionResult<List<EmployeeDto>>> GetAll()
	{
		var response = await _mediator.Send(new GetEmployeesQuery());
		return Ok(response);
	}

	/// <summary>
	/// Registers a new employee based on the provided command.
	/// </summary>
	/// <param name="command">The command containing information to register a new employee.<br></br>
	/// <br>Role ID:</br>
	/// <b>1</b>-Manager
	/// <br><b>2</b> -Worker</br>
	/// </param>
	/// <response code="400">If data is invalid or email already exists.</response>
	/// <response code="204">If employee is successfully created.</response>
	[HttpPost("register")]
	[Authorize(Roles ="Manager")]
	[SwaggerResponse(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
	public async Task<ActionResult> Register(RegisterEmployeeCommand command)
	{
		await _mediator.Send(command);
		return NoContent();
	}


	/// <summary>
	/// Logs in an employee based on the provided credentials.
	/// </summary>
	/// <param name="command">The command containing an email and password.</param>
	/// <returns>The JWT token.</returns>
	/// <response code="200">If email and password are correct.</response>
	/// <response code="400">If email or password validation failed.</response>
	/// <response code="404">If email or password authentication failed.</response>
	[HttpPost("login")]
	[ProducesResponseType(typeof(string),StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
	public async Task<ActionResult<string>> Login(SignInCommand command)
	{
		var response = await _mediator.Send(command);
		return Ok(response.Token);
	}

	/// <summary>
	/// Updates the password of the authenticated employee.
	/// </summary>
	/// <param name="command">The command containing the new password and password confirmation</param>
	/// <response code="204">If password was changed.</response>
	/// <response code="400">If password or confirmation password validation failed.</response>
	[HttpPut("update_password")]
	[Authorize(Roles ="Manager,Worker")]
	[SwaggerResponse(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(typeof(string),StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
	public async Task<IActionResult> UpdatePassword(UpdatePasswordCommand command)
	{
		await _mediator.Send(command);
		return NoContent();
	}
}
