using BikeWorkshop.Application.Interfaces.Services;
using BikeWorkshop.Infrastructure.Authentication;
using BikeWorkshop.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BikeWorkshop.Infrastructure.DI;

internal static class JwtExtension
{
	public static IServiceCollection AddJwtService(this IServiceCollection services,IConfiguration configuration)
	{
		var authSettings = new AuthenticationSettings();
		configuration.GetSection("Authentication").Bind(authSettings);

		services.AddSingleton(authSettings);
		services.AddScoped<IJwtService,JwtService>();
		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = "Bearer";
			options.DefaultScheme = "Bearer";
			options.DefaultChallengeScheme = "Bearer";
		}).AddJwtBearer(cfg =>
		{
			cfg.RequireHttpsMetadata = false;
			cfg.SaveToken = true;
			cfg.TokenValidationParameters = new TokenValidationParameters()
			{
				ValidIssuer = authSettings.JwtIssuer,
				ValidAudience = authSettings.JwtIssuer,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding
				.UTF8.GetBytes(authSettings.JwtKey)),
			};
		});

		services.AddScoped<IRefreshTokenService,RefreshTokenService>();	

		return services;
	}
}
