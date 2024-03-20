using FluentValidation;
using System.Text.RegularExpressions;

namespace BikeWorkshop.Application.Fluent_Validation_Extensions;

internal static class EmailValidationExtension
{
	private const string pattern = "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$";
	public static IRuleBuilder<T, string> EmailMustBeValid<T>(this IRuleBuilder<T, string> ruleBuilder)
	{
		return ruleBuilder
			.NotNull().WithMessage("Email can't be null!")
			.NotEmpty().WithMessage("Email can't be empty!")
			.Must(email => Regex.IsMatch(email, pattern)).WithMessage("Invalid email pattern!");
	}

	public static IRuleBuilder<T, string?> NullableEmailMustBeValid<T>(this IRuleBuilder<T, string?> ruleBuilder)
	{
		return ruleBuilder
			.Must((email) =>
			{
				var result = true;
				if (email is not null)
				{
					result = Regex.IsMatch(email, pattern);
				}
				return result;
			}).WithMessage("Invalid email pattern!");
	}


}
