using BikeWorkshop.API.Tests.Settings;
using BikeWorkshop.API.Tests.Settings.BaseClasses;
using BikeWorkshop.API.Tests.Settings.WebAppFactories;
using BikeWorkshop.Application.Functions.DTO;
using FluentAssertions;
using Newtonsoft.Json;

namespace BikeWorkshop.API.Tests.ClientData;
public class GetOrderTests 
    : ManagerBaseClass
{
	public GetOrderTests(ManagerTestWebApplicationFactory<Program> factory) : base(factory)
    {
	}
    private async void Init()
    {
		await dbContext.Initialize();
	}

	[Fact]
    public async Task GetByOrderId_UnknownOrderId_ReturnNotFoundStatus()
    {
        var orderId = Guid.NewGuid();
        var response = await httpClient.GetAsync($"api/ClientData/{Guid.NewGuid}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
	public async Task GetByOrderId_ValidOrderId_ReturnsOKStatus()
	{
        var orderId = Guid.NewGuid();
		await dbContext.AddOrderAndClient(orderId, Constants.ManagerId);

		var response = await httpClient.GetAsync($"api/ClientData/{orderId}");
        var responseContent = JsonConvert.DeserializeObject<ClientDataDto>(await response.Content.ReadAsStringAsync());

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseContent?.PhoneNumber.Should().NotBeNull();
	}
}
