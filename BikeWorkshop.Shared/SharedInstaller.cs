using BikeWorkshop.Shared.MiddleWare;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BikeWorkshop.Shared;

public static class SharedInstaller
{
	public static IServiceCollection AddShared(this IServiceCollection services)
	{
		services.AddScoped<ExceptionMiddleware>();
		return services;
	}
	public static IApplicationBuilder UseShared(this IApplicationBuilder builder)
	{
		builder.UseMiddleware<ExceptionMiddleware>();
		return builder;
	}
}
