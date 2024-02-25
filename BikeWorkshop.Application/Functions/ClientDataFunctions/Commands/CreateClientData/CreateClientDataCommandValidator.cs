using BikeWorkshop.Application.Fluent_Validation_Extensions;
using FluentValidation;

namespace BikeWorkshop.Application.Functions.ClientDataFunctions.Commands.CreateClientData;

internal class CreateClientDataCommandValidator
    : AbstractValidator<CreateClientDataCommand>
{
    public CreateClientDataCommandValidator()
    {
        RuleFor(x => x.Email)
            .NullableEmailMustBeValid();
    }
}
