using FluentValidation.Results;
using Newtonsoft.Json;

namespace BikeWorkshop.Application.Fluent_Validation_Extensions;
file record ValidationErrorInfo(string PropertyName,string Message);

internal static class JsonSerializer
{
	public static string ToJsonString(this List<ValidationFailure> validationResult)
	{
		var errorToList = validationResult.Select(v=> new ValidationErrorInfo(
			v.PropertyName
			,v.ErrorMessage)).ToList();
		var jsonString = JsonConvert.SerializeObject(errorToList,Formatting.Indented);
		return jsonString;
	}
}
