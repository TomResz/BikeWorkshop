using BikeWorkshop.API.Tests.Settings.BaseClasses;
using BikeWorkshop.API.Tests.Settings.WebAppFactories;
using BikeWorkshop.Application.Functions.EmployeeFunctions.Commands.UpdatePassword;
using FluentAssertions;
using Newtonsoft.Json;

namespace BikeWorkshop.API.Tests.Employee.UpdatePassword;
public class UpdatePasswordByUnauthorizedUserTests
	: UnauthorizedUserBaseClass
{
	public UpdatePasswordByUnauthorizedUserTests(UnauthorizedTestWebApplicationFactory<Program> factory) : base(factory)
	{
	}

	[Fact]
	public async Task UpdatePassword_UnauthorizedUser_ReturnsUnauthorizedStatus()
	{
		var command = new UpdatePasswordCommand("12345678","12345678");
		var json = JsonConvert.SerializeObject(command);
		var httpContent = new StringContent(json,System.Text.Encoding.UTF8,"application/json");

		var response = await httpClient.PostAsync("api/employee/update-password", httpContent);

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
	}
}
