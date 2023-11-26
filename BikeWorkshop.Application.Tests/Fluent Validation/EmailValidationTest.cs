using BikeWorkshop.Application.Fluent_Validation_Extensions;
using FluentValidation;
using FluentValidation.TestHelper;

namespace BikeWorkshop.Application.Tests.Fluent_Validation;

public class EmailValidationTest
{
	private class TestClass
	{
        public string Email { get; set; }
    }
	private class TestClassValidator : AbstractValidator<TestClass>
	{
        public TestClassValidator()
        {
            RuleFor(x => x.Email)
                .EmailMustBeValid();
        }
    }

	[Theory]
	[InlineData("valid.email@example.com")]
	[InlineData("another.valid.email@example.co.uk")]
	[InlineData("user@domain.com")]
	public void EmailMustBeValid_ShouldNotHaveValidationErrorsForValidEmails(string validEmail)
	{
		// Arrange
		var validator = new TestClassValidator();
		var testObject = new TestClass { Email = validEmail };

		// Act
		var result = validator.TestValidate(testObject);

		// Assert
		Assert.True(result.IsValid);
		result.ShouldNotHaveValidationErrorFor(x => x.Email);
	}

	[Theory]
	[InlineData("invalid.email")]
	[InlineData("invalid.email@")]
	[InlineData("invalid.email@domain")]
	[InlineData("")]
	public void EmailMustBeValid_ShouldHaveValidationErrorsForInvalidEmails(string invalidEmail)
	{
		// Arrange
		var validator = new TestClassValidator();
		var testObject = new TestClass { Email = invalidEmail };

		// Act
		var result = validator.TestValidate(testObject);

		// Assert
		Assert.False(result.IsValid);
		result.ShouldHaveValidationErrorFor(x => x.Email);
	}

}
