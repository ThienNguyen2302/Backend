using TDTU.API.Dtos.RegistrationStatusDTO;

namespace TDTU.API.Interfaces;

public interface IRegistrationStatusService
{
	Task<PaginatedList<RegistrationStatusDto>> GetPagination(PaginationRequest request);
	Task<List<RegistrationStatusDto>> GetAll(BaseRequest request);
	Task<List<RegistrationStatusDto>> GetFilter(FilterRequest request);
}
