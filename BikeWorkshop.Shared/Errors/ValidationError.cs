namespace BikeWorkshop.Shared.Errors;
public sealed class ValidationError
{
    public string PropertyName { get; set; }
    public string Message { get; set; }
}
