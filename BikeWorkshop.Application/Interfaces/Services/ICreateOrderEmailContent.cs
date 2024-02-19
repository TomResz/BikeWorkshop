namespace BikeWorkshop.Application.Interfaces.Services;
public interface ICreateOrderEmailContent
{
	string Content(string url, string shortId);
}
