using BikeWorkshop.API.Tests.Settings.WebAppFactories;
using BikeWorkshop.Infrastructure.EF.Context;
using Microsoft.Extensions.DependencyInjection;

namespace BikeWorkshop.API.Tests.Settings.BaseClasses;
public abstract class ManagerBaseClass
	: IClassFixture<ManagerTestWebApplicationFactory<Program>>
{
	protected readonly HttpClient httpClient;
	protected readonly ManagerTestWebApplicationFactory<Program> applicationFactory;
	protected readonly BikeWorkshopDbContext dbContext;

    public ManagerBaseClass(ManagerTestWebApplicationFactory<Program> managerTestWeb)
    {
		applicationFactory = managerTestWeb;
		httpClient = applicationFactory.CreateClient();
		var scopeFactory = applicationFactory.Services.GetService<IServiceScopeFactory>();
		var scope = scopeFactory!.CreateScope();
		dbContext = scope.ServiceProvider.GetRequiredService<BikeWorkshopDbContext>();
	}
}
