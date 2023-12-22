using FluentValidation;

namespace BikeWorkshop.Application.Fluent_Validation_Extensions;

internal static class PhoneNumberValidationExtension
{
	private const string pattern = @"(\+\d{2}\d{2})?\d{9}";
	public static IRuleBuilder<T,string> PhoneNumberMustBeValid<T>(this IRuleBuilder<T,string> ruleBuilder)
	{
		return ruleBuilder
			.NotEmpty().WithMessage("Can't be empty!")
			.Must(pn=> !pn.Any(char.IsLetter))
			.Matches(pattern).WithMessage("Invalid phone number pattern!");
	}
}
