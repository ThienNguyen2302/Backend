using TDTU.API.Dtos.RegularJobDTO;
using TDTU.API.Models.InternshipJobModel;
using TDTU.API.Models.RegularJobModel;

namespace TDTU.API.Interfaces;

public interface IRegularJobService
{
	Task<PaginatedList<RegularJobDto>> GetPagination(JobPaginationRequest request);
	Task<PaginatedList<RegularJobDto>> Suggest(SuggestRequest request);
	Task<List<RegularJobDto>> GetAll(BaseRequest request);
	Task<List<RegularJobDto>> GetFilter(FilterRequest request);
	Task<RegularJobDto> GetById(Guid id, Guid? userId = null);
	Task<bool> DeleteByIds(DeleteRequest request);
	Task<RegularJobDto> Add(RegularJobAddOrUpdate request);
	Task<RegularJobDto> Update(RegularJobAddOrUpdate request);
}
