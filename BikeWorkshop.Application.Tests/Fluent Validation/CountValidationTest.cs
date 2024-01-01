using BikeWorkshop.Application.Fluent_Validation_Extensions;
using FluentValidation;
using FluentValidation.TestHelper;

namespace BikeWorkshop.Application.Tests.Fluent_Validation;

file record CountTestObject(int Count);
file class CountTest : AbstractValidator<CountTestObject>
{
    public CountTest()
    {
        RuleFor(x => x.Count)
            .CountMustBeValid();
    }
}
public class CountValidationTest
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public void CountMustBeValid_ValidData_ShouldNotHaveAnyErrors(int count)
    {
        var validator = new CountTest();
        var testObj = new CountTestObject(count);
        
        var result = validator.TestValidate(testObj);

        Assert.True(result.IsValid);
        result.ShouldNotHaveAnyValidationErrors();
    }


	[Theory]
	[InlineData(0)]
	[InlineData(-2)]
	[InlineData(-3)]
	[InlineData(-4)]
	public void CountMustBeValid_InvalidData_ShouldNotHaveAnyErrors(int count)
	{
		var validator = new CountTest();
		var testObj = new CountTestObject(count);

		var result = validator.TestValidate(testObj);

		Assert.False(result.IsValid);
		result.ShouldHaveAnyValidationError();
		result.ShouldHaveValidationErrorFor(x=>x.Count);
	}
}
