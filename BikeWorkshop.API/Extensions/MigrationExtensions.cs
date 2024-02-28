using BikeWorkshop.Infrastructure.EF.Context;
using Microsoft.EntityFrameworkCore;

namespace BikeWorkshop.API.Extensions;
public static class MigrationExtensions
{
	public static void ApplyMigration(this IApplicationBuilder app)
	{
		using var scope = app.ApplicationServices.CreateScope();
		using var dbContext = scope.ServiceProvider.GetRequiredService<BikeWorkshopDbContext>();
		dbContext.Database.Migrate();
	}
}
