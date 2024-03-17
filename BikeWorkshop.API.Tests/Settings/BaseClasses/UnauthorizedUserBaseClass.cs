using BikeWorkshop.API.Tests.Settings.WebAppFactories;
using BikeWorkshop.Infrastructure.EF.Context;
using Microsoft.Extensions.DependencyInjection;

namespace BikeWorkshop.API.Tests.Settings.BaseClasses;
public abstract class UnauthorizedUserBaseClass
	: IClassFixture<UnauthorizedTestWebApplicationFactory<Program>>
{
	protected readonly HttpClient httpClient;
	protected readonly UnauthorizedTestWebApplicationFactory<Program> applicationFactory;
	protected readonly BikeWorkshopDbContext dbContext;
	public UnauthorizedUserBaseClass(UnauthorizedTestWebApplicationFactory<Program> factory)
    {
		applicationFactory = factory;
		httpClient = applicationFactory.CreateClient();
		var scopeFactory = applicationFactory.Services.GetService<IServiceScopeFactory>();
		var scope = scopeFactory!.CreateScope();
		dbContext = scope.ServiceProvider.GetRequiredService<BikeWorkshopDbContext>();
	}
}
