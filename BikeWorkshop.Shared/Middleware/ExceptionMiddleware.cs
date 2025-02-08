using BikeWorkshop.Shared.Errors;
using BikeWorkshop.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Error = BikeWorkshop.Shared.Errors.Error;

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
        try
        {
            await next.Invoke(context);
        }
        catch (ValidationException ex)
        {
            await HandleValidationException(context, ex, 400);
        }
        catch (BadRequestException ex)
        {
            await HandleAnotherExceptions(context, ex, 400);
        }
        catch (UnauthorizedException ex)
        {
            await HandleAnotherExceptions(context, ex, 401);
        }
        catch (NotFoundException ex)
        {
            await HandleAnotherExceptions(context, ex, 404);
        }
        catch (Exception ex)
        {
            await HandleAnotherExceptions(context, ex, 500);
        }
    }

    private async Task HandleValidationException(HttpContext context, ValidationException ex, int statusCode)
    {
        var errors = ex.Errors;
        var errorObject = new ErrorWithValidationParameters
        {
            Code = statusCode,
            Type = "Validation Error",
            Errors = errors
        };
        _logger.LogError("Exception occurred: {Message} {@Errors} {@Exception} ", errorObject.Type, errors, ex);
        var content = System.Text.Json.JsonSerializer.Serialize(errorObject);
        await WriteMessageAsync(context, content, statusCode);

    }
    private async Task HandleAnotherExceptions(HttpContext context, Exception ex, int statusCode)
    {
        var error = ex.Message;

        var errorType = statusCode switch
        {
            400 => "Bad Request",
            404 => "Not Found",
            _ => "Internal Server Error"

        };
        var errorObject = new Error
        {
            Code = statusCode,
            Type = errorType,
            Message = error
        };
        _logger.LogError("Exception occurred: {Message} {@Errors} {@Exception} ", errorType, error, ex);

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            _logger.LogError($"Critical error stack trace: {ex.StackTrace}.");
        }

        var content = System.Text.Json.JsonSerializer.Serialize(errorObject);
        await WriteMessageAsync(context, content, statusCode);

    }

    private static async Task WriteMessageAsync(HttpContext context, string content, int statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsync(content);
    }

}

