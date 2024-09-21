namespace TDTU.API.Models;

public class PaginationRequest : BaseRequest
{
	public int PageIndex { get; init; } = 1;
	public int PageSize { get; init; } = 10;
	
}
