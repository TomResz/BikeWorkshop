using FluentValidation;

namespace BikeWorkshop.Application.Functions.OrderFunctions.Command.CreateOrder;

internal class CreateOrderCommandValidator
    : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Can't be empty!");
    }
}
