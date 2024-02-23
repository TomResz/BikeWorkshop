using BikeWorkshop.Application.Interfaces.Email;

namespace BikeWorkshop.Infrastructure.Email.Contents;
internal sealed class SummaryOrderEmailContent : ISummaryEmailContent
{
	public string Content(decimal totalAmount)
		=> $@"<p style=""font-family: 'Times New Roman', Times, serif; font-size: 25px;"">Your order  has been completed!</p>
			<p style=""font-family: 'Times New Roman', Times, serif; font-size: 25px;"">Total count: <b style=""font-style: italic;"">{Math.Round(totalAmount, 2)}</b>.</p>";
}
