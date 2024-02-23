namespace BikeWorkshop.Application.Interfaces.Email;
public interface ICreateOrderEmailContent
{
    string Content(string url, string shortId);
}
