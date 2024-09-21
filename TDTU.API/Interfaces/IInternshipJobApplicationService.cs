using TDTU.API.Dtos.InternshipJobApplicationDTO;
using TDTU.API.Models.InternshipJobApplicationModel;

namespace TDTU.API.Interfaces;

public interface IInternshipJobApplicationService
{
	Task<InternshipJobApplicationDto> Apply(InternshipJobApply request);
	Task<InternshipJobApplicationDto> SetStatus(InternshipJobSetStatus request);
	Task<PaginatedList<InternshipJobApplicationDto>> UserHistory(ApplicationPaginationRequest request);
	Task<PaginatedList<InternshipJobApplicationDto>> JobApplications(PaginationRequest request, Guid id);
	Task<InternshipJobApplicationDto> GetById(Guid id);
	Task<bool> DeleteByIds(DeleteRequest request);
}
