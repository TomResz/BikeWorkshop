using FluentValidation;

namespace BikeWorkshop.Application.Fluent_Validation_Extensions;

internal static class NameValidationExtension
{
	private const string Pattern = @"[^\p{L}\s]*";
	public static IRuleBuilder<T,string> NameMustBeValid<T>(this IRuleBuilder<T,string> ruleBuilder,
		string propertyName)
	{
		return ruleBuilder
			.NotNull()
			.NotEmpty().WithMessage($"Empty {propertyName}!")
			.Must(name=>!name.Any(char.IsDigit))
			.Matches(Pattern).WithMessage($"Incorrect {propertyName}!");
	}
}
