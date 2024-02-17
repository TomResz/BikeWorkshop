using FluentValidation;

namespace BikeWorkshop.Application.Functions.SummaryFunctions.Command.CreateSummaryForOrder;
internal class CreateSummaryForOrderCommandValidator
	: AbstractValidator<CreateSummaryForOrderCommand>
{
    public CreateSummaryForOrderCommandValidator()
    {
        RuleFor(x => x.Conclusion)
            .Must(con =>
            {
                if(con is not null
                    && con.Length > 200)
                {
                    return false;
                }
                return true;
            }).WithMessage("Exceded maximum length!");
    }
}
