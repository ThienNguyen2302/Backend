using TDTU.API.Dtos.InternshipRegistrationDTO;
using TDTU.API.Models.InternshipRegistrationModel;

namespace TDTU.API.Interfaces;

public interface IInternshipRegistrationService
{
	Task<PaginatedList<InternshipRegistrationDto>> GetPagination(PaginationRequest request);
	Task<bool> DeleteByIds(DeleteRequest request);
	Task<InternshipRegistrationDto> GetById(Guid id);
	Task<PaginatedList<InternshipRegistrationDto>> GetPendingStudent(BaseRequest request);
	Task<InternshipRegistrationDto> Add(InternshipRegistrationAddOrUpdate request);
	Task<InternshipRegistrationDto> Update(InternshipRegistrationAddOrUpdate request);
}
