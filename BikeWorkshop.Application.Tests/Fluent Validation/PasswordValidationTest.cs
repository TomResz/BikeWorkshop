using BikeWorkshop.Application.Fluent_Validation_Extensions;
using FluentValidation;
using FluentValidation.TestHelper;

namespace BikeWorkshop.Application.Tests.Fluent_Validation;

file record PasswordTestClass(string Password);

file class PasswordValidatorTest : AbstractValidator<PasswordTestClass>
{
    public PasswordValidatorTest()
    {
        RuleFor(x => x.Password)
            .PasswordMustBeValid();
    }
}

public class PasswordValidationTest
{
    [Theory]
    [InlineData("pls")]
    [InlineData("asdfg")]
    [InlineData("example")]
    [InlineData("1234567")]
    public void PasswordMustBeValid_InvalidPassword_ShouldHaveValidationErrors(string password)
    {
        var validator = new PasswordValidatorTest();
        var testObject = new PasswordTestClass(password);

        var result = validator.TestValidate(testObject);
        Assert.False(result.IsValid);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }


	[Theory]
	[InlineData("examplepassword")]
	[InlineData("123456789")]
	[InlineData("asdfghjkl")]
	[InlineData("qwertyuiop")]
	public void PasswordMustBeValid_ValidPassword_ShouldHaveValidationErrors(string password)
	{
		var validator = new PasswordValidatorTest();
		var testObject = new PasswordTestClass(password);

		var result = validator.TestValidate(testObject);
		Assert.True(result.IsValid);
		result.ShouldNotHaveValidationErrorFor(x => x.Password);
	}
}
