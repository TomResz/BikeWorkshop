using BikeWorkshop.API.Tests.Settings.FakeServices;
using BikeWorkshop.Application.Interfaces.Services;
using BikeWorkshop.Infrastructure.EF.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BikeWorkshop.API.Tests.Settings.WebAppFactories.Common;
public static class ServiceExtensions
{
	public static IServiceCollection AddCommonsServices(this IServiceCollection services)
	{
        Environment.SetEnvironmentVariable("APITest", "true");
        var dbContextOpt = services.SingleOrDefault(
	s => s.ServiceType == typeof(DbContextOptions<BikeWorkshopDbContext>))!;
		services.Remove(dbContextOpt);

		var smtpService = services.First(s => s.ServiceType == typeof(ICustomEmailSender));
		services.Remove(smtpService);
		services.AddSingleton<ICustomEmailSender, FakeSMTPService>();
		var db = $"BikeWorkshopDb{Guid.NewGuid()}";

		services.AddDbContext<BikeWorkshopDbContext>(
			d => d.UseInMemoryDatabase(db));
		return services;
	}
}
