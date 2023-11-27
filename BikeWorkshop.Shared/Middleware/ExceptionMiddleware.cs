using BikeWorkshop.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BikeWorkshop.Shared.MiddleWare;

public class ExceptionMiddleware : IMiddleware
{
	private readonly ILogger<ExceptionMiddleware> _logger;

	public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
	{
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		context.Response.ContentType = "application/json";
		try
		{
			await next.Invoke(context);
		}
		catch (NotFoundException ex)
		{
			await HandleException(context, ex, 404);
		}
		catch (BadRequestException ex)
		{
			await HandleException(context, ex, 400);
		}
		catch(UnauthorizedException ex)
		{
			await HandleException(context, ex,401);
		}
		catch (Exception ex)
		{
			await HandleException(context, ex, 500);
		}
	}

	private async Task HandleException(HttpContext context, Exception ex, int statusCode)
	{
		_logger.LogError($"{GetLogMessage(ex, statusCode)}");

		context.Response.StatusCode = statusCode;
		if (string.IsNullOrEmpty(ex.Message))
		{
			await context.Response.WriteAsync("");
			return;
		}

		if ( IsJson(ex.Message))
		{
			await context.Response.WriteAsync(ex.Message);
			return;
		}
		await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new { error = ex.Message }));

	}

	private bool IsJson(string value)
	{
		try
		{
			JToken.Parse(value);
			return true;
		}
		catch (JsonReaderException)
		{
			return false;
		}
		catch(Exception)
		{
			return false;
		}
	}

	private string GetLogMessage(Exception ex, int statusCode) 
		=> statusCode == 500
			? $"Internal Server Error: {ex.Message}"
			: $"Error {statusCode}: {ex.Message}";
}
