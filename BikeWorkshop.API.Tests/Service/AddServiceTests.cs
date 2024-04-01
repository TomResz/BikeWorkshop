using BikeWorkshop.API.Tests.Settings.BaseClasses;
using BikeWorkshop.API.Tests.Settings.DatabaseFilters;
using BikeWorkshop.API.Tests.Settings.WebAppFactories;
using BikeWorkshop.Application.Functions.ServiceFunctions.Commands.Add;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BikeWorkshop.API.Tests.Service;
public class AddServiceTests
	: ManagerBaseClass
{
	public AddServiceTests(ManagerTestWebApplicationFactory<Program> managerTestWeb) 
		: base(managerTestWeb)
	{
	}

	[Fact]
	public async Task Add_ValidData_ReturnsCreatedStatus()
	{
		var name = "Example name";
		var command = new AddServiceCommand(name);
		var json = JsonSerializer.Serialize(command);
		var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

		var response = await httpClient.PostAsync("api/service/add", httpContent);

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
		var service = await dbContext.Services.Where(x=>x.Name == name).FirstOrDefaultAsync();
		service.Should().NotBeNull();
	}

	[Fact]
	public async Task Add_NotUniqueName_ReturnsConflictStatus()
	{
		await dbContext.Initialize();
		var service = await dbContext.AddServices();
		var serviceName = service.Select(x=>x.Name).FirstOrDefault()!;
		var command = new AddServiceCommand(serviceName);
		var json = JsonSerializer.Serialize(command);
		var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

		var response = await httpClient.PostAsync("api/service/add", httpContent);

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.Conflict);
	}

	[Fact]
	public async Task Add_InvalidName_ReturnsBadRequestStatus()
	{
		var command = new AddServiceCommand("");
		var json = JsonSerializer.Serialize(command);
		var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

		var response = await httpClient.PostAsync("api/service/add", httpContent);

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
	}
}
