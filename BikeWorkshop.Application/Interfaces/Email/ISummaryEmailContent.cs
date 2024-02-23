namespace BikeWorkshop.Application.Interfaces.Email;
public interface ISummaryEmailContent
{
    string Content(decimal totalAmount);
}
