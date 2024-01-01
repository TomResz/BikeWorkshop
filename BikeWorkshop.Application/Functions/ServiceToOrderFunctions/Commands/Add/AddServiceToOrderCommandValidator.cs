using BikeWorkshop.Application.Fluent_Validation_Extensions;
using FluentValidation;

namespace BikeWorkshop.Application.Functions.ServiceToOrderFunctions.Commands.Add;

internal class AddServiceToOrderCommandValidator
	: AbstractValidator<AddServiceToOrderCommand>
{
    public AddServiceToOrderCommandValidator()
    {
        RuleFor(x => x.Price)
            .PriceMustBeValid();
        RuleFor(x=>x.Count)
            .CountMustBeValid();
    }
}
