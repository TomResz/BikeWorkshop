using BikeWorkshop.Application.Fluent_Validation_Extensions;
using BikeWorkshop.Shared.Exceptions;
using FluentValidation;
using MediatR;
namespace BikeWorkshop.Application.MediatorPipeline;

public sealed class ValidationBehavior<TRequest, TResponse>
	: IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseRequest
{
	private readonly IEnumerable<IValidator<TRequest>> _validators;

	public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
	{
		_validators = validators;
	}

	public async Task<TResponse> Handle(TRequest request,
		RequestHandlerDelegate<TResponse> next,
		CancellationToken cancellationToken)
	{
		var context = new ValidationContext<TRequest>(request);

		var validationFailures = await Task.WhenAll(
			_validators.Select(validator => validator.ValidateAsync(context)));

		if (validationFailures.Any())
		{
			var errors = validationFailures
				.Where(validationResult => !validationResult.IsValid)
				.SelectMany(validationResult => validationResult.Errors)
				.ToList();
			if (errors.Any())
			{
				throw new BadRequestException(errors.ToJsonString());
			}
		}
		return await next();
	}
}
