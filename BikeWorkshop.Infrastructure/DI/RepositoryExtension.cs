using BikeWorkshop.Application.Interfaces.Repositories;
using BikeWorkshop.Infrastructure.EF.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BikeWorkshop.Infrastructure.DI;

internal static class RepositoryExtension
{
	public static IServiceCollection AddRepositories(this IServiceCollection services)
		=> 
		services.AddScoped<IEmployeeRepository, EmployeeRepository>()
			.AddScoped<IOrderRepository, OrderRepository>()
			.AddScoped<IClientDataRepository, ClientDataRepository>()
			.AddScoped<IServiceRepository, ServiceRepository>()
			.AddScoped<IServiceToOrderRepository,ServiceToOrderRepository>()
			.AddScoped<ISummaryRepository,SummaryRepository>();

}
