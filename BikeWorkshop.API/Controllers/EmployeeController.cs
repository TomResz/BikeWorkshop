using BikeWorkshop.Application.Functions.EmployeeFunctions.Commands.Register;
using BikeWorkshop.Application.Functions.EmployeeFunctions.Commands.SignIn;
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

}
