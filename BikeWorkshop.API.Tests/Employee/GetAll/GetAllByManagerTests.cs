using BikeWorkshop.API.Tests.Settings.BaseClasses;
using BikeWorkshop.API.Tests.Settings.WebAppFactories;
using BikeWorkshop.Application.Functions.DTO;
using FluentAssertions;
using System.Text.Json;

namespace BikeWorkshop.API.Tests.Employee.GetAll;
public class GetAllByManagerTests
	: ManagerBaseClass
{
	public GetAllByManagerTests(ManagerTestWebApplicationFactory<Program> managerTestWeb) : base(managerTestWeb)
	{
	}

	[Fact]
	public async Task GetAll_AuthorizedManager_ReturnsOKStatusCodeAndEmployees()
	{
		var count = await dbContext.AddEmployees();

		var response = await httpClient.GetAsync("api/employee/all");

		var employees = JsonSerializer.Deserialize<List<EmployeeDto>>(await response.Content.ReadAsStringAsync());

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
		employees?.Count().Should().Be(count);
	}
}
