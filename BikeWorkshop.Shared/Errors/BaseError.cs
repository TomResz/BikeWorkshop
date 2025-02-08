namespace BikeWorkshop.Shared.Errors;

public class BaseError
{
    public int Code { get; set; }
    public string Type { get; set; }
}

public class Error : BaseError
{
    public string Message { get; set; }
}

public class ErrorWithValidationParameters : BaseError
{
    public List<ValidationError> Errors { get; set; }
}