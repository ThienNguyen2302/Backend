using TDTU.API.Dtos.InternshipJobDTO;
using TDTU.API.Models.InternshipJobModel;
using TDTU.API.Models.InternshipRegistrationModel;

namespace TDTU.API.Interfaces;

public interface IInternshipJobService
{
	Task<PaginatedList<InternshipJobDto>> GetPagination(JobPaginationRequest request);
	Task<PaginatedList<InternshipJobDto>> Suggest(SuggestRequest request);
	Task<List<InternshipJobDto>> GetAll(BaseRequest request);
	Task<List<InternshipJobDto>> GetFilter(FilterRequest request);
	Task<InternshipJobDto> GetById(Guid id, Guid? userId = null);
	Task<bool> DeleteByIds(DeleteRequest request);
	Task<bool> AttachStudent(AttachStudentToJob request);
	Task<InternshipJobDto> Add(InternshipJobAddOrUpdate request);
	Task<InternshipJobDto> Update(InternshipJobAddOrUpdate request);
}
