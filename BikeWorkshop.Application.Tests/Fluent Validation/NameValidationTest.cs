using BikeWorkshop.Application.Fluent_Validation_Extensions;
using FluentValidation;
using FluentValidation.TestHelper;

namespace BikeWorkshop.Application.Tests.Fluent_Validation;

file record NameTestClass(string Name);
file class NameValidator : AbstractValidator<NameTestClass>
{
    public NameValidator()
    {
        RuleFor(x => x.Name)
            .NameMustBeValid("name");
    }
}

public class NameValidationTest
{
    [Theory]
    [InlineData("name")]
    [InlineData("tomt")]
    [InlineData("example name ")]
    [InlineData("polish chars: ąęźżó")]
    public void NameMustBeValid_ValidName_ShouldNotHaveValidationErrors(string name)
    {
        var validator = new NameValidator();    
        var testObject = new NameTestClass(name);

        var validationResult = validator.TestValidate(testObject);

        Assert.True(validationResult.IsValid);
        validationResult.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
	[InlineData("")]
	[InlineData("example n1ame ")]
	[InlineData("polish chars: ąęźżó 123 ")]
	public void NameMustBeValid_InvalidName_ShouldHaveValidationErrors(string name)
    {
        var validator = new NameValidator();
        var testObject = new NameTestClass(name);

        var validationResult = validator.TestValidate(testObject);
        Assert.False(validationResult.IsValid);
        validationResult.ShouldHaveValidationErrorFor(x=>x.Name);
    }
}
