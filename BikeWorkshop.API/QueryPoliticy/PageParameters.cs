using System.ComponentModel.DataAnnotations;

namespace BikeWorkshop.API.QueryPoliticy;

public class PageParameters
{
	[Required]
	[Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0.")]
	public int Page { get; init; }
	[Required]
	[Range(1, int.MaxValue, ErrorMessage = "Page size number must be greater than 0.")]
	public int PageSize { get; init; }
}
