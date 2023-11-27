using BikeWorkshop.Application.Functions.DTO;
using BikeWorkshop.Application.Functions.EmployeeFunctions.Commands.Register;
using BikeWorkshop.Application.Functions.EmployeeFunctions.Commands.SignIn;
using BikeWorkshop.Application.Functions.EmployeeFunctions.Commands.UpdatePassword;
using BikeWorkshop.Application.Functions.EmployeeFunctions.Queries.GetEmployees;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BikeWorkshop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
	private readonly IMediator _mediator;

	public EmployeeController(IMediator mediator)
	{
		_mediator = mediator;
	}
	[HttpGet("getall")]
	public async Task<ActionResult<List<EmployeeDto>>> GetAll()
	{
		var response = await _mediator.Send(new GetEmployeesQuery());
		return Ok(response);
	}


	[HttpPost("register")]
	public async Task<ActionResult> Register(RegisterEmployeeCommand command)
	{
		await _mediator.Send(command);
		return Ok();
	}

	[HttpPost("login")]
	public async Task<ActionResult<string>> Login(SignInCommand command)
	{
		var response = await _mediator.Send(command);
		return Ok(response.Token);
	}

	
	[HttpPut("updatePassword")]
	public async Task<ActionResult> UpdatePassword(UpdatePasswordCommand command)
	{
		await _mediator.Send(command);
		return Ok();
	}
}
