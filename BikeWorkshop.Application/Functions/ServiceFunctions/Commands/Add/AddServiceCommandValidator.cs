using BikeWorkshop.Application.Fluent_Validation_Extensions;
using FluentValidation;

namespace BikeWorkshop.Application.Functions.ServiceFunctions.Commands.Add;

internal class AddServiceCommandValidator
	: AbstractValidator<AddServiceCommand>
{
    public AddServiceCommandValidator()
    {
        RuleFor(x => x.Name)
            .NameMustBeValid("name");
    }
}
