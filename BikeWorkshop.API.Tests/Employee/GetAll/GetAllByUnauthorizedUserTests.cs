using BikeWorkshop.API.Tests.Settings.BaseClasses;
using BikeWorkshop.API.Tests.Settings.WebAppFactories;
using FluentAssertions;

namespace BikeWorkshop.API.Tests.Employee.GetAll;
public class GetAllByUnauthorizedUserTests
	: UnauthorizedUserBaseClass
{
	public GetAllByUnauthorizedUserTests(UnauthorizedTestWebApplicationFactory<Program> factory) : base(factory)
	{
	}

	[Fact]
	public async Task GetAll_UnauthorizedUser_ReturnsUnauthorizedStatus()
	{
		var count = await dbContext.AddEmployees();

		var response = await httpClient.GetAsync("api/employee/all");

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);		
	}
}
