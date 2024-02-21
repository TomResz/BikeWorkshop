using BikeWorkshop.Application.Functions.DTO.Enums;
using BikeWorkshop.Shared.Exceptions;

namespace BikeWorkshop.API.QueryPoliticy;

public class SortingParameters
{
	public static SortingDirection FromString(string direction)
	{
		if (direction.ToLower() == "asc")
			return SortingDirection.Ascending;
		else if (direction.ToLower() == "desc")
			return SortingDirection.Descending;
		throw new BadRequestException("Invalid sorting direction! Use ascending or descending!");
	}
}
