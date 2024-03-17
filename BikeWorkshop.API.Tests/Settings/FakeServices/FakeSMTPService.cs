using BikeWorkshop.Application.Interfaces.Services;

namespace BikeWorkshop.API.Tests.Settings.FakeServices;
public class FakeSMTPService : ICustomEmailSender
{
    public Task SendEmailAsync(string emailOfReceiver, string subject, string htmlBody)
    {
        return Task.CompletedTask;
    }
}
