using FluentValidation;

namespace BikeWorkshop.Application.Fluent_Validation_Extensions;

internal static class NameValidationExtension
{
	private const string Pattern = "^[A-Za-zĄąĆćĘęŁłŃńÓóŚśŹźŻż]+$";
	public static IRuleBuilder<T,string> NameMustBeValid<T>(this IRuleBuilder<T,string> ruleBuilder,
		string propertyName)
	{
		return ruleBuilder
			.NotNull()
			.NotEmpty().WithMessage($"Empty {propertyName}!")
			.Matches(Pattern).WithMessage($"Incorrect {propertyName}!");
	}
}
