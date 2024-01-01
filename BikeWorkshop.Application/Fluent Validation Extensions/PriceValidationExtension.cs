using FluentValidation;

namespace BikeWorkshop.Application.Fluent_Validation_Extensions;

internal static class PriceValidationExtension
{
	public static IRuleBuilder<T, decimal> PriceMustBeValid<T>(this IRuleBuilder<T, decimal> ruleBuilder)
	{
		return ruleBuilder
			.Must((price) => price > 0m)
			.WithMessage("Invalid price value!");
	}
}
