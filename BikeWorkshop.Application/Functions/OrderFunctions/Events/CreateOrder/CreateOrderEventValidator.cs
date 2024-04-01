using BikeWorkshop.Application.Fluent_Validation_Extensions;
using FluentValidation;

namespace BikeWorkshop.Application.Functions.OrderFunctions.Events.CreateOrder;
internal sealed class CreateOrderEventValidator 
	: AbstractValidator<CreateOrderEvent>
{
    public CreateOrderEventValidator()
    {
        RuleFor(x => x.Email)
            .NullableEmailMustBeValid();
        RuleFor(x=> x.PhoneNumber)
            .PhoneNumberMustBeValid();
    }
}
