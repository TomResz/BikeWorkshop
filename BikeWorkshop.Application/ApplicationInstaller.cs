using BikeWorkshop.Application.Email.Contents;
using BikeWorkshop.Application.Interfaces.Services;
using BikeWorkshop.Application.MediatorPipeline;
using BikeWorkshop.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BikeWorkshop.Application;

public static class ApplicationInstaller
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddScoped<IPasswordHasher<Employee>,PasswordHasher<Employee>>();
		services.AddMediatR(cfg =>
		{
			cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
		});
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
		services.AddSingleton<ICreateOrderEmailContent, CreateOrderEmailContent>();
		services.AddSingleton<ISummaryEmailContent, SummaryOrderEmailContent>();
		services.AddValidators();
		return services;
	}
}
