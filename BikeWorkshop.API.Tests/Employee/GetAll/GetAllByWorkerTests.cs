using BikeWorkshop.API.Tests.Settings.BaseClasses;
using BikeWorkshop.API.Tests.Settings.WebAppFactories;
using FluentAssertions;

namespace BikeWorkshop.API.Tests.Employee.GetAll;
public class GetAllByWorkerTests
	: WorkerBaseClass
{
	public GetAllByWorkerTests(WorkerTestWebApplicationFactory<Program> workerTestWeb) : base(workerTestWeb)
	{
	}

	[Fact]
	public async Task GetAll_AuthorizedWorker_ReturnsForbiddenStatus()
	{
		var count = await dbContext.AddEmployees();

		var response = await httpClient.GetAsync("api/employee/all");

		response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
	}
}
