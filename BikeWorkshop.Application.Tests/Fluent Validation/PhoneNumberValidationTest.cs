using BikeWorkshop.Application.Fluent_Validation_Extensions;
using FluentValidation;
using FluentValidation.TestHelper;

namespace BikeWorkshop.Application.Tests.Fluent_Validation;

file record PhoneNumberObject(string PhoneNumber);
file class PhoneNumberValidator : AbstractValidator<PhoneNumberObject>
{
    public PhoneNumberValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .PhoneNumberMustBeValid();
    }
}
public class PhoneNumberValidationTest
{

	[Theory]
	[InlineData("666777888")]
	[InlineData("+48666777888")]
	[InlineData("+48666777999")]
	[InlineData("+49666777999")]
    public void PhoneNumberMustBeValid_ValidPhoneNumber_ShouldNotHaveValidationErrors(string phoneNumber)
    {
        var validator = new PhoneNumberValidator();
        var testObject = new PhoneNumberObject(phoneNumber);

        var resultOfValidation = validator.TestValidate(testObject);

        Assert.True(resultOfValidation.IsValid);
        resultOfValidation.ShouldNotHaveAnyValidationErrors();
    }


	[Theory]
	[InlineData("6667778")]
	[InlineData("+48as666777888")]
	[InlineData("+4866ds6777999")]
	[InlineData("")]
	public void PhoneNumberMustBeValid_InvalidPhoneNumber_ShouldHaveValidationErrors(string phoneNumber)
	{
		var validator = new PhoneNumberValidator();
		var testObject = new PhoneNumberObject(phoneNumber);

		var resultOfValidation = validator.TestValidate(testObject);

		Assert.False(resultOfValidation.IsValid);
		resultOfValidation.ShouldHaveAnyValidationError();
	}
}
