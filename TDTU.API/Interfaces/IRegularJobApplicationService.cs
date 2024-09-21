using TDTU.API.Dtos.RegularJobApplicationDTO;
using TDTU.API.Models.RegularJobApplicationModel;

namespace TDTU.API.Interfaces;

public interface IRegularJobApplicationService
{
	Task<RegularJobApplicationDto> ApplyJob(RegularJobApplyRequest request);
	Task<PaginatedList<RegularJobApplicationDto>> JobApplications(PaginationRequest request, Guid id);
	Task<PaginatedList<RegularJobApplicationDto>> UserHistory(ApplicationPaginationRequest request);
	Task<RegularJobApplicationDto> GetById(Guid id);
	Task<bool> DeleteByIds(DeleteRequest request);
}
