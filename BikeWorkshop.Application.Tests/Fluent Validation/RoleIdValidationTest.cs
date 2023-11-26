using BikeWorkshop.Application.Fluent_Validation_Extensions;
using BikeWorkshop.Domain.Enums;
using FluentValidation;
using FluentValidation.TestHelper;
using System.Drawing;

namespace BikeWorkshop.Application.Tests.Fluent_Validation;

file class TestClass
{
    public int RoleId { get; set; }
}
file class TestClassValidator : AbstractValidator<TestClass>
{
    public TestClassValidator()
    {
        RuleFor(x => x.RoleId)
            .RoleIdMustBeValid();
    }
}
public class RoleIdValidationTest
{
	private class TestClass
	{
		public int RoleId { get; set; }
	}
	private class TestClassValidator : AbstractValidator<TestClass>
	{
		public TestClassValidator()
		{
			RuleFor(x => x.RoleId)
				.RoleIdMustBeValid();
		}
	}

	[Theory]
	[InlineData((int)Roles.Worker)]
	[InlineData((int)Roles.Manager)]
	public void RoleIdMustBeValid_ShouldNotHaveValidationErrors(int RoleId)
	{
		// arrange
		var validator = new TestClassValidator();
		var testClass = new TestClass() { RoleId = RoleId};
		// act
		var resultOfVal = validator.TestValidate(testClass);
		// assert
		Assert.True(resultOfVal.IsValid);
		resultOfVal.ShouldNotHaveValidationErrorFor(x => x.RoleId);
	}
	[Theory]
	[InlineData(15)]
	[InlineData(0)]
	[InlineData(3)]
	public void RoleIdMustBeValid_ShouldHaveValidationErrors(int RoleId)
	{
		// arrange
		var validator = new TestClassValidator();
		var testClass = new TestClass() { RoleId = RoleId };
		// act
		var resultOfVal = validator.TestValidate(testClass);
		// assert
		Assert.False(resultOfVal.IsValid);
		resultOfVal.ShouldHaveValidationErrorFor(x => x.RoleId);
	}


}
