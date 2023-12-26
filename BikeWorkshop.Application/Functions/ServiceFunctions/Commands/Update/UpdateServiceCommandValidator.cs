using FluentValidation;

namespace BikeWorkshop.Application.Functions.ServiceFunctions.Commands.Update;

internal class UpdateServiceCommandValidator 
	: AbstractValidator<UpdateServiceCommand>
{
	public UpdateServiceCommandValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("Name can't be empty!");
	}
}
