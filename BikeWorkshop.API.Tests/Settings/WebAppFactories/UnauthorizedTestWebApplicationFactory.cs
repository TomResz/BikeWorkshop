using BikeWorkshop.API.Tests.Settings.WebAppFactories.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace BikeWorkshop.API.Tests.Settings.WebAppFactories;

public class UnauthorizedTestWebApplicationFactory<TStartup> 
    : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable(variable: "APITest", "true");
        builder.UseEnvironment("APITest");

        builder.ConfigureServices(services =>
        {
            services.AddCommonsServices();
            builder.UseEnvironment("APITest");

        });
    }
}
