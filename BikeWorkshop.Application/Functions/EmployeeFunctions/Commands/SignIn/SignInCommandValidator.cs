﻿using BikeWorkshop.Application.Fluent_Validation_Extensions;
using FluentValidation;

namespace BikeWorkshop.Application.Functions.EmployeeFunctions.Commands.SignIn;

internal class SignInCommandValidator : AbstractValidator<SignInCommand>
{
    public SignInCommandValidator()
    {
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .EmailMustBeValid();

        RuleFor(x=>x.Password) 
            .Cascade(CascadeMode.Stop)
            .PasswordMustBeValid();
    }
}
