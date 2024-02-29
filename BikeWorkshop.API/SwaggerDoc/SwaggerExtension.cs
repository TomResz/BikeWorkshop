using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace BikeWorkshop.API.SwaggerDoc;

public static class SwaggerExtension
{
	public static IServiceCollection AddSwaggerDocSettings(this IServiceCollection services)
	{
		services.AddSwaggerGen(c =>
		{
			c.SwaggerDoc("v1", new OpenApiInfo { Title = "BikeWorkshop API", Version = "v1" });
			var securityScheme = new OpenApiSecurityScheme
			{
				Name = "JWT Authentication",
				Description = "Enter your JWT token",
				In = ParameterLocation.Header,
				Type = SecuritySchemeType.Http,
				Scheme = "bearer",
				BearerFormat = "JWT",
				Reference = new OpenApiReference
				{
					Id = JwtBearerDefaults.AuthenticationScheme,
					Type = ReferenceType.SecurityScheme
				}
			};
			c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);
			c.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{ securityScheme, new string[] { } }
			});
			var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
			var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
			c.IncludeXmlComments(xmlPath);
			c.EnableAnnotations();
		});
		return services;
	}
}
