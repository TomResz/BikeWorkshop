using FluentValidation;

namespace BikeWorkshop.Application.Fluent_Validation_Extensions;

internal static class CountValidationExtension
{
	public static IRuleBuilder<T, int> CountMustBeValid<T>(this IRuleBuilder<T, int> ruleBuilder) => ruleBuilder
			.Must((count) => count > 0).WithMessage("Invalid number of count!");
}
