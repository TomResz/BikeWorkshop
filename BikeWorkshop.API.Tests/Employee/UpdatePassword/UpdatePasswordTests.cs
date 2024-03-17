using BikeWorkshop.API.Tests.Settings.BaseClasses;
using BikeWorkshop.API.Tests.Settings.DatabaseFilters;
using BikeWorkshop.API.Tests.Settings.WebAppFactories;
using BikeWorkshop.Application.Functions.EmployeeFunctions.Commands.UpdatePassword;
using FluentAssertions;
using Newtonsoft.Json;
using System.Text;

namespace BikeWorkshop.API.Tests.Employee;
public class UpdatePasswordTests 
	: ManagerBaseClass
{
	private const string _path = "api/Employee/update_password";
	public UpdatePasswordTests(ManagerTestWebApplicationFactory<Program> manager) : base(manager)
    {
		Init();
	}
	private async void Init() => await dbContext.Initialize();

	[Fact]
	public async Task UpdatePassword_AuthorizedManager_ShouldUpdatePassword()
	{
		var command = new UpdatePasswordCommand("123456789", "123456789");
		var json = JsonConvert.SerializeObject(command);
		var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
		
		var response = await httpClient.PutAsync(_path, httpContent);
		var responseContent = await response.Content.ReadAsStringAsync();


		response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
		responseContent.Should().BeNullOrEmpty();
	}

	[Fact]
	public async Task UpdatePassword_InvalidPasswordPattern_ReturnsBadRequestStatus()
	{
		var password = "1234567";
		var command = new UpdatePasswordCommand(password,password);
		var json = JsonConvert.SerializeObject(command);
		var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

		var response = await httpClient.PutAsync(_path, httpContent);
		var responseContent = await response.Content.ReadAsStringAsync();	

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
		responseContent.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task UpdatePassword_InvalidConfirmedPassword_ReturnsBadRequestStatus()
	{
		var confirmedPassword = "123456789";
		var command = new UpdatePasswordCommand("12345678", confirmedPassword);
		var json = JsonConvert.SerializeObject(command);
		var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

		var response = await httpClient.PutAsync(_path, httpContent);
		var responseContent = await response.Content.ReadAsStringAsync();

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
		responseContent.Should().NotBeNullOrEmpty();
	}
}
