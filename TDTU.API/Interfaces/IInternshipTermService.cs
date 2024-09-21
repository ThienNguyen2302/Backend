using TDTU.API.Dtos.InternshipTermDTO;
using TDTU.API.Models.InternshipTermModel;

namespace TDTU.API.Interfaces;

public interface IInternshipTermService
{
	Task<PaginatedList<InternshipTermDto>> GetPagination(PaginationRequest request);
	Task<List<InternshipTermDto>> GetAll(BaseRequest request);
	Task<List<InternshipTermDto>> GetFilter(FilterRequest request);
	Task<InternshipTermDto> GetById(Guid id);
	Task<bool> DeleteByIds(DeleteRequest request);
	Task<InternshipTermDto> Add(InternshipTermAddOrUpdate request);
	Task<InternshipTermDto> Update(InternshipTermAddOrUpdate request);
}
