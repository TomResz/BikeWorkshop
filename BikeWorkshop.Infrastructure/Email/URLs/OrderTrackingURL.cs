using BikeWorkshop.Application.Interfaces.Email;

namespace BikeWorkshop.Infrastructure.Email.URLs;
public class OrderTrackingURL : IOrderTrackingURL
{
	public string Url { get; set; }
}
