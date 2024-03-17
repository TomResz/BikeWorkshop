using BikeWorkshop.API.Tests.Settings.Policy;
using BikeWorkshop.API.Tests.Settings.WebAppFactories.Common;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace BikeWorkshop.API.Tests.Settings.WebAppFactories;
public class ManagerTestWebApplicationFactory<TStartup> 
	: WebApplicationFactory<TStartup> where TStartup : class
{
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureServices(services =>
		{
			services.AddCommonsServices();
			services.AddSingleton<IPolicyEvaluator, FakeManagerPolicyEvaluator>();
		});
	}
}
