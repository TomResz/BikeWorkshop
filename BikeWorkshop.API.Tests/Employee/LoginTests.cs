using BikeWorkshop.API.Tests.Settings;
using BikeWorkshop.API.Tests.Settings.BaseClasses;
using BikeWorkshop.API.Tests.Settings.WebAppFactories;
using BikeWorkshop.Application.Functions.EmployeeFunctions.Commands.SignIn;
using FluentAssertions;
using Newtonsoft.Json;
using System.Text;

namespace BikeWorkshop.API.Tests.Employee;
public class LoginTests : UnauthorizedUserBaseClass
{
	public LoginTests(UnauthorizedTestWebApplicationFactory<Program> testWeb) : base(testWeb)
	{
	}

	[Fact]
	public async Task Login_ValidCredentials_ReturnsToken()
	{
		await dbContext.Initialize();

		var loginCommand = new SignInCommand(Constants.ManagerEmail, Constants.Password);	
		var json = JsonConvert.SerializeObject(loginCommand);
		var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

		var response = await httpClient.PostAsync($"api/Employee/login", httpContent);
		var token = await response.Content.ReadAsStringAsync();

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
		token.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task Login_InvalidEmail_ReturnsBadRequestStatus()
	{
		await dbContext.Initialize();

		var email = "John";
		var loginCommand = new SignInCommand(email, Constants.Password);
		var json = JsonConvert.SerializeObject(loginCommand);
		var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

		var response = await httpClient.PostAsync($"api/Employee/login", httpContent);
		var feedback = await response.Content.ReadAsStringAsync();

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
		feedback.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task Login_InvalidPassword_ReturnsNotFoundStatus()
	{
		await dbContext.Initialize();

		var password = "johnny@johny";
		// valid password: 12345678
		var loginCommand = new SignInCommand(Constants.ManagerEmail, password);
		var json = JsonConvert.SerializeObject(loginCommand);
		var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

		var response = await httpClient.PostAsync($"api/Employee/login", httpContent);
		var feedback = await response.Content.ReadAsStringAsync();

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
		feedback.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task Login_InvalidPasswordLength_ReturnsNotFoundStatus()
	{
		await dbContext.Initialize();

		var password = "";
		// valid password: 12345678
		var loginCommand = new SignInCommand(Constants.ManagerEmail, password);
		var json = JsonConvert.SerializeObject(loginCommand);
		var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

		var response = await httpClient.PostAsync($"api/Employee/login", httpContent);
		var feedback = await response.Content.ReadAsStringAsync();

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
		feedback.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task Login_UnknownEmail_ReturnsNotFoundStatus()
	{
		await dbContext.Initialize();
		// valid password: 12345678
		var loginCommand = new SignInCommand("manager@email.com", Constants.Password);
		var json = JsonConvert.SerializeObject(loginCommand);
		var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

		var response = await httpClient.PostAsync($"api/Employee/login", httpContent);
		var feedback = await response.Content.ReadAsStringAsync();

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
		feedback.Should().NotBeNullOrEmpty();
	}

}
