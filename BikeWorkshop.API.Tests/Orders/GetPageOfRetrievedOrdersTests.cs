using BikeWorkshop.API.Tests.Extensions;
using BikeWorkshop.API.Tests.Settings.BaseClasses;
using BikeWorkshop.API.Tests.Settings.DatabaseFilters;
using BikeWorkshop.API.Tests.Settings.WebAppFactories;
using FluentAssertions;

namespace BikeWorkshop.API.Tests.Orders;
public class GetPageOfRetrievedOrdersTests
	: WorkerBaseClass
{
	public GetPageOfRetrievedOrdersTests(WorkerTestWebApplicationFactory<Program> workerTestWeb) : base(workerTestWeb)
	{
	}

	[Theory]
	[InlineData(1, 5, "asc")]
	[InlineData(2, 10, "desc")]
	public async Task GetPageOfRetrieved_ValidParameters_Returns200Status(int page, int pageSize, string direction)
	{
		await dbContext.Initialize();
		await dbContext.AddOrdersWithRetrievedStatus();
		var response = await httpClient.GetAsync($"api/order/retrieved?page={page}&pageSize={pageSize}&direction={direction}");
		var json = await response.Content.ReadAsStringAsync();
		var pagedList = PagedListDeserialization.DeserializeToOrderDto(json);

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
		pagedList.PageSize.Should().Be(pageSize);
		pagedList.Page.Should().Be(page);
		pagedList.Items.Count().Should().Be(pageSize);
		
		switch(direction)
		{
			case "asc":
				pagedList.Items.Should().BeInAscendingOrder(x => x.AddedDate);
				break;
			case "desc":
				pagedList.Items.Should().BeInDescendingOrder(x => x.AddedDate);
				break;
		};
	}

	[Theory]
	[InlineData(0, 5)]
	[InlineData(2, 0)]
	public async Task GetPageOfRetrieved_InvalidParameters_ReturnsBadRequestStatus(int page, int pageSize)
	{
		await dbContext.Initialize();
		await dbContext.AddOrdersWithRetrievedStatus();
		var response = await httpClient.GetAsync($"api/order/retrieved?page={page}&pageSize={pageSize}");
		response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

	}
}
