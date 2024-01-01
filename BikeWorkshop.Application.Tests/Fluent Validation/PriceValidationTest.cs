using BikeWorkshop.Application.Fluent_Validation_Extensions;
using FluentValidation;
using FluentValidation.TestHelper;

namespace BikeWorkshop.Application.Tests.Fluent_Validation;

file record PriceTestObject(decimal Price);
file class PriceTestValidator 
	: AbstractValidator<PriceTestObject>
{
	public PriceTestValidator()
	{
		RuleFor(x => x.Price)
			.PriceMustBeValid();
	}
}
public class PriceValidationTest
{
	[Theory]
	[InlineData(1)]
	[InlineData(10)]
	[InlineData(0.001)]
	[InlineData(100.25)]
	[InlineData(1367.5)]
	public void PriceMustBeValid_ValidPrice_ShouldNotHaveErrors(decimal price)
	{
		// Arrange
		var priceObj = new PriceTestObject(price);
		var validator = new PriceTestValidator();

		// Act
		var result = validator.TestValidate(priceObj);

		// Assert
		result.ShouldNotHaveValidationErrorFor(x => x.Price);
		Assert.True(result.IsValid);
	}
	[Theory]
	[InlineData(0)]
	[InlineData(-1)]
	[InlineData(-0.001)]
	[InlineData(-100.25)]
	[InlineData(-1367.5)]
	public void PriceMustBeValid_InvalidPrice_ShouldHaveValidationErrors(decimal price)
	{
		// Arrange
		var priceObj = new PriceTestObject(price);
		var validator = new PriceTestValidator();

		// Act
		var result = validator.TestValidate(priceObj);

		// Assert
		result.ShouldHaveAnyValidationError();
		result.ShouldHaveValidationErrorFor(x => x.Price);
		Assert.False(result.IsValid);
	}
}
