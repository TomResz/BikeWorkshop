using BikeWorkshop.Application.Interfaces.Services;
using BikeWorkshop.Infrastructure.SMPT;
using System.Net.Mail;

namespace BikeWorkshop.Infrastructure.Services;

public sealed class CustomEmailSender : ICustomEmailSender
{
	private readonly SmtpClient _smtpClient;
	private readonly SMTPClientSettings _smtpSettings;

	public CustomEmailSender(SmtpClient smtpClient, SMTPClientSettings smtpSettings)
	{
		_smtpClient = smtpClient;
		_smtpSettings = smtpSettings;
	}

	public async Task SendEmailAsync(string emailOfReceiver, string subject, string htmlBody)
	{
		var mailMessage = new MailMessage
		{
			From = new MailAddress(_smtpSettings.Email),
			Subject = subject,
			Body = htmlBody,
			IsBodyHtml = true,
		};
		mailMessage.To.Add(emailOfReceiver);
		await _smtpClient.SendMailAsync(mailMessage);
	}
}
