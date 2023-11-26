using BikeWorkshop.Application.Fluent_Validation_Extensions;
using FluentValidation;

namespace BikeWorkshop.Application.Functions.EmployeeFunctions.Commands.Register;

public class RegisterEmployeeValidator
    : AbstractValidator<RegisterEmployeeCommand>
{
    public RegisterEmployeeValidator()
    {
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .EmailMustBeValid();

        RuleFor(x=>x.RoleId)
            .RoleIdMustBeValid();

        RuleFor(x => x.FirstName)
            .NameMustBeValid("first name");

		RuleFor(x => x.LastName)
	        .NameMustBeValid("last name");
	}
}
