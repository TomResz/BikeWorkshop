using BikeWorkshop.Application.Interfaces.Email;
using BikeWorkshop.Application.Interfaces.Services;
using BikeWorkshop.Infrastructure.Email.Contents;
using BikeWorkshop.Infrastructure.Email.URLs;
using BikeWorkshop.Infrastructure.Services;
using BikeWorkshop.Infrastructure.SMPT;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Mail;

namespace BikeWorkshop.Infrastructure.DI;

internal static class SMTPExtension
{
	public static IServiceCollection AddSMTPClient(this IServiceCollection services,IConfiguration configuration)
	{
		var smptClientSettings = new SMTPClientSettings();
		configuration.GetSection("SMPT").Bind(smptClientSettings);
		var smptClient = new SmtpClient(smptClientSettings.Host)
		{
			Port = smptClientSettings.Port,
			Credentials = new NetworkCredential(smptClientSettings.Email, smptClientSettings.Password),
			EnableSsl = true,
		};
		services.AddSingleton(smptClient);
		services.AddSingleton(smptClientSettings);
		services.AddSingleton<ICustomEmailSender, CustomEmailSender>();

		var trackingUrl = new OrderTrackingURL();
		configuration.GetSection("TrackingUrl").Bind(trackingUrl);
		services.AddSingleton(typeof(IOrderTrackingURL),trackingUrl);
		services.AddSingleton<ICreateOrderEmailContent, CreateOrderEmailContent>();
		services.AddSingleton<ISummaryEmailContent, SummaryOrderEmailContent>();
		return services;
	}
}
