using BikeWorkshop.Application.Fluent_Validation_Extensions;
using FluentValidation;

namespace BikeWorkshop.Application.Functions.EmployeeFunctions.Commands.UpdatePassword;

internal class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
{
    public UpdatePasswordCommandValidator()
    {
        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .PasswordMustBeValid();
        RuleFor(x=>x.ConfirmedPassword) 
            .Must((pass,cp) => cp == pass.Password).WithMessage("Password don't match!");
    }
}
