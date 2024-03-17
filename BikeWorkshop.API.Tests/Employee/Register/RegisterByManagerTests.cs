using BikeWorkshop.API.Tests.Settings;
using BikeWorkshop.API.Tests.Settings.BaseClasses;
using BikeWorkshop.API.Tests.Settings.WebAppFactories;
using BikeWorkshop.Application.Functions.EmployeeFunctions.Commands.Register;
using BikeWorkshop.Domain.Enums;
using FluentAssertions;
using System.Text.Json;

namespace BikeWorkshop.API.Tests.Employee.Register;
public class RegisterByManagerTests
	: ManagerBaseClass
{
	private const string _path = "api/employee/register";

	private readonly RegisterEmployeeCommand _command = new("John", "White",
			"johny@johny.com", "12345678", "12345678", (int)Roles.Worker);
	public RegisterByManagerTests(ManagerTestWebApplicationFactory<Program> managerTestWeb) : base(managerTestWeb)
	{
	}

	[Fact]
	public async Task Register_ValidData_ShouldRegisterNewEmployee()
	{
		await dbContext.Initialize();

		var json = JsonSerializer.Serialize(_command);

		var httpContent = new StringContent(json,System.Text.Encoding.UTF8,"application/json");

		var response = await httpClient.PostAsync(_path, httpContent);

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

	}


	[Fact]
	public async Task Register_InvalidRoleId_ReturnsBadRequestStatus()
	{
		await dbContext.Initialize();

		var command = _command with { RoleId = 3 };
		var json = JsonSerializer.Serialize(command);

		var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

		var response = await httpClient.PostAsync("api/employee/register", httpContent);
		var message = await response.Content.ReadAsStringAsync();


		response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
		message.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task Register_NotUniqueEmail_ReturnsBadRequestStatus()
	{
		await dbContext.Initialize();

		var command = _command with { Email = Constants.ManagerEmail };
		var json = JsonSerializer.Serialize(command);

		var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

		var response = await httpClient.PostAsync("api/employee/register", httpContent);
		var message = await response.Content.ReadAsStringAsync();


		response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
		message.Should().NotBeNullOrEmpty();
	}


	[Fact]
	public async Task Register_InvalidConfirmedPassword_ReturnsBadRequestStatus()
	{
		await dbContext.Initialize();

		var command = _command with { ConfirmedPassword = "123" };
		var json = JsonSerializer.Serialize(command);

		var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

		var response = await httpClient.PostAsync(_path, httpContent);
		var message = await response.Content.ReadAsStringAsync();


		response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
		message.Should().NotBeNullOrEmpty();
	}
	[Fact]
	public async Task Register_InvalidPasswordPattern_ReturnsBadRequestStatus()
	{
		await dbContext.Initialize();

		var command = _command with { Password = "123" };
		var json = JsonSerializer.Serialize(command);

		var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

		var response = await httpClient.PostAsync(_path, httpContent);
		var message = await response.Content.ReadAsStringAsync();


		response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
		message.Should().NotBeNullOrEmpty();
	}
	[Fact]
	public async Task Register_EmptyFirstName_ReturnsBadRequestStatus()
	{
		await dbContext.Initialize();

		var command = _command with { FirstName = "" };
		var json = JsonSerializer.Serialize(command);

		var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

		var response = await httpClient.PostAsync(_path, httpContent);
		var message = await response.Content.ReadAsStringAsync();


		response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
		message.Should().NotBeNullOrEmpty();
	}
	[Fact]
	public async Task Register_EmptyLastName_ReturnsBadRequestStatus()
	{
		await dbContext.Initialize();

		var command = _command with { LastName = "" };
		var json = JsonSerializer.Serialize(command);

		var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

		var response = await httpClient.PostAsync(_path, httpContent);
		var message = await response.Content.ReadAsStringAsync();


		response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
		message.Should().NotBeNullOrEmpty();
	}


	[Theory]
	[InlineData("")]
	[InlineData("john")]
	[InlineData("john@john")]
	[InlineData("john@john.")]
	public async Task Register_InvalidEmailPattern_ReturnsBadRequestStatus(string email)
	{
		await dbContext.Initialize();

		var command = _command with { Email = email };
		var json = JsonSerializer.Serialize(command);

		var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

		var response = await httpClient.PostAsync(_path, httpContent);
		var message = await response.Content.ReadAsStringAsync();


		response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
		message.Should().NotBeNullOrEmpty();
	}
}
