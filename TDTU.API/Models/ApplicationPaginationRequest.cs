namespace TDTU.API.Models;

public class ApplicationPaginationRequest : PaginationRequest
{
	public Guid? CompanyId { get; set; }
}
