namespace BikeWorkshop.Application.Interfaces.Services;

public interface ICustomEmailSender
{
	Task SendEmailAsync(string emailOfReceiver,string subject,string htmlBody);
}
