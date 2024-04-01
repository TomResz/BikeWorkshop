using BikeWorkshop.API.Tests.Settings.BaseClasses;
using BikeWorkshop.API.Tests.Settings.WebAppFactories;
using BikeWorkshop.Application.Functions.DTO;
using FluentAssertions;
using System.Text.Json;

namespace BikeWorkshop.API.Tests.Service;
public class GetAllServicesTests
	: ManagerBaseClass
{
	public GetAllServicesTests(ManagerTestWebApplicationFactory<Program> managerTestWeb)
		: base(managerTestWeb)
	{
	}

	[Theory]
	[InlineData("desc")]
	[InlineData("asc")]
	[InlineData(null)]
	public async Task GetAll_ValidParameters_ReturnsOKStatus(string? direction) 
	{
		var services = await dbContext.AddServices();
		var sufix = direction is null ? string.Empty : $"?direction={direction}";
		var response = await httpClient.GetAsync($"api/service/all{sufix}");
		
		var serviceList = JsonSerializer.Deserialize<List<ServiceDto>>(await response.Content.ReadAsStringAsync());
		
		response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
		serviceList.Should().NotBeNull();
		serviceList?.Count().Should().Be(services.Count());	
	}

	[Theory]
	[InlineData("dessc")]
	[InlineData("ascc")]
	public async Task GetAll_InvalidDirection_ReturnsBadRequestStatus(string direction)
	{
		var services = await dbContext.AddServices();

		var response = await httpClient.GetAsync($"api/service/all?direction={direction}");

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
	}
}