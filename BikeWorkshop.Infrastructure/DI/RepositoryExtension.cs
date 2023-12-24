using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Infrastructure.EF.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BikeWorkshop.Infrastructure.DI;

internal static class RepositoryExtension
{
	public static IServiceCollection AddRepositories(this IServiceCollection services)
	{
		services.AddScoped<IEmployeeRepository, EmployeeRepository>();
		services.AddScoped<IOrderRepository, OrderRepository>();
		services.AddScoped<IClientDataRepository, ClientDataRepository>();
		services.AddScoped<IServiceRepository, ServiceRepository>();
		return services;
	}
}
