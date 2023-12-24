namespace BikeWorkshop.Application.Functions.ServiceFunctions.Commands.Add;

public record AddServiceResponse(
	bool IsAdded,
	string? Feedback);
