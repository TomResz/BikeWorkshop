using BikeWorkshop.Shared.Errors;
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
        if (_validators.Any())
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

                var errorToList = errors.Select(x => new Shared.Errors.ValidationError
                {
                    Message = x.ErrorMessage,
                    PropertyName = x.PropertyName,
                }).ToList();

                if (errors.Any())
                {
                    throw new Shared.Exceptions.ValidationException(errorToList);
                }
            }
        }
        return await next();
    }
}
