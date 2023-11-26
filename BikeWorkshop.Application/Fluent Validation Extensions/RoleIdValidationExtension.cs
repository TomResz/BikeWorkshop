using BikeWorkshop.Domain.Enums;
using FluentValidation;

namespace BikeWorkshop.Application.Fluent_Validation_Extensions;

internal static class RoleIdValidationExtension
{
	public static IRuleBuilder<T,int> RoleIdMustBeValid<T>(this IRuleBuilder<T,int> ruleBuilder)
	{
		return ruleBuilder
			.Must(x => Enum.IsDefined(typeof(Roles), x)).WithMessage("Incorrect role Id!");
	}
}
