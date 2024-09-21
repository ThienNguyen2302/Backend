using TDTU.API.Dtos.ApplicationStatusDTO;

namespace TDTU.API.Interfaces;

public interface IApplicationStatusService
{
	Task<PaginatedList<ApplicationStatusDto>> GetPagination(PaginationRequest request);
	Task<List<ApplicationStatusDto>> GetAll(BaseRequest request);
	Task<List<ApplicationStatusDto>> GetFilter(FilterRequest request);
}
