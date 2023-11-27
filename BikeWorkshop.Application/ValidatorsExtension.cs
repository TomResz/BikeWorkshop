using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BikeWorkshop.Application;

internal static class ValidatorsExtension
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        var validatorType = typeof(IValidator<>);

        var validatorTypes = Assembly
            .GetAssembly(typeof(ValidatorsExtension))
            .GetTypes()
            .Where(type => type.GetInterfaces()
                .Any(interfaceType => interfaceType.IsGenericType &&
                                      interfaceType.GetGenericTypeDefinition() == validatorType))
            .ToList();

        foreach (var validator in validatorTypes)
        {
            var interfaceType = validator.GetInterfaces()
                .First(type => type.IsGenericType && type.GetGenericTypeDefinition() == validatorType);

            services.AddTransient(interfaceType, validator);
        }

        return services;
    }
}
