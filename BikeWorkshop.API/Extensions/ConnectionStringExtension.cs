namespace BikeWorkshop.API.Extensions;

public class ConnectionStringExtension
{
	public static string GetConnectionString(IConfiguration configuration)
	{
		var isRunningDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");
		if(!string.IsNullOrEmpty(isRunningDocker) && isRunningDocker is "true")
		{
			return configuration.GetConnectionString("Docker") 
				?? throw new InvalidDataException("Unknown connection string for docker container!");
		}
		return configuration.GetConnectionString("LocalDb") 
			?? throw new InvalidDataException("Unknown connection string for local database!");
	}
}
