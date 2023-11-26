using BikeWorkshop.Application.Functions.EmployeeFunctions.Commands.Register;
using BikeWorkshop.Domain.Entities;
using FluentValidation;
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
			cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
		services.AddValidatorsFromAssemblyContaining<RegisterEmployeeValidator>();
		return services;
	}
}
