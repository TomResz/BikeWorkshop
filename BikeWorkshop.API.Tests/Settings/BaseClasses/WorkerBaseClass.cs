using BikeWorkshop.API.Tests.Settings.WebAppFactories;
using BikeWorkshop.Infrastructure.EF.Context;
using Microsoft.Extensions.DependencyInjection;

namespace BikeWorkshop.API.Tests.Settings.BaseClasses;
public abstract class WorkerBaseClass
    : IClassFixture<WorkerTestWebApplicationFactory<Program>>

{
    protected readonly HttpClient httpClient;
    protected readonly WorkerTestWebApplicationFactory<Program> applicationFactory;
    protected readonly BikeWorkshopDbContext dbContext;
    public WorkerBaseClass(WorkerTestWebApplicationFactory<Program> workerTestWeb)
    {
        applicationFactory = workerTestWeb;
        httpClient = applicationFactory.CreateClient();
        var scopeFactory = applicationFactory.Services.GetService<IServiceScopeFactory>();
        var scope = scopeFactory!.CreateScope();
        dbContext = scope.ServiceProvider.GetRequiredService<BikeWorkshopDbContext>();
    }
}
