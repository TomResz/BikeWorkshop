using FluentValidation;

namespace BikeWorkshop.Application.Fluent_Validation_Extensions;

internal static class PasswordValidationExtension
{
	public static IRuleBuilder<T,string> PasswordMustBeValid<T>(this IRuleBuilder<T,string> ruleBuilder)
	{
		return ruleBuilder
			.NotEmpty().WithMessage("Password can't be empty!")
			.MinimumLength(8).WithMessage("Minimum size is equal 8!");
	}
}
