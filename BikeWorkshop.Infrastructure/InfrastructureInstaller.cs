using BikeWorkshop.Infrastructure.DI;
using BikeWorkshop.Infrastructure.EF.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BikeWorkshop.Infrastructure;

public static class InfrastructureInstaller
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
	{
		services.AddDbContext<BikeWorkshopDbContext>(options =>
		{
			options.UseSqlServer(configuration.GetConnectionString("LocalDb"));
		});
		services.AddHttpContextAccessor();
		services.AddJwtService(configuration);
		services.AddRepositories();
		return services;
	}
}
