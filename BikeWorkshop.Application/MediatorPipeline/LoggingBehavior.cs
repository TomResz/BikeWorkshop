using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BikeWorkshop.Application.MediatorPipeline;
public sealed class LoggingBehavior<TRequest, TResponse>
	: IPipelineBehavior<TRequest, TResponse>
{
	private readonly ILogger<LoggingBehavior<TRequest,TResponse>> _logger;
	public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
	{
		_logger = logger;
	}

	public async Task<TResponse> Handle(
		TRequest request,
		RequestHandlerDelegate<TResponse> next,
		CancellationToken cancellationToken)
	{
		var stopwatch = new Stopwatch();
		_logger.LogInformation($"Started handling {typeof(TRequest).Name}.");
		stopwatch.Start();
		var response = await next();
		stopwatch.Stop();
		_logger.LogInformation($"Ended handling {typeof(TRequest).Name} with time: {stopwatch.ElapsedMilliseconds}ms.");
		return response;
	}
}
