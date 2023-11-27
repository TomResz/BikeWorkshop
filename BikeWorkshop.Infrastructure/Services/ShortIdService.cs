using BikeWorkshop.Application.Interfaces.Services;
using shortid;
using shortid.Configuration;

namespace BikeWorkshop.Infrastructure.Services;

public class ShortIdService : IShortIdService
{
	public string GetShortId()
	{
		var options = new GenerationOptions(length: 9);
		return ShortId.Generate(options);
	}
}
