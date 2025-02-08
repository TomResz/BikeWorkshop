using BikeWorkshop.Shared.Errors;

namespace BikeWorkshop.Shared.Exceptions;
public sealed class ValidationException(List<ValidationError> errors) : Exception
{
    public List<ValidationError> Errors = errors;
}
