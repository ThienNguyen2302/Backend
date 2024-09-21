using TDTU.API.Dtos.SkillDTO;
using TDTU.API.Models.SkillModel;

namespace TDTU.API.Interfaces;

public interface ISkillService
{
	Task<PaginatedList<SkillDto>> GetPagination(PaginationRequest request);
	Task<List<SkillDto>> GetAll(BaseRequest request);
	Task<List<SkillDto>> GetFilter(FilterRequest request);
	Task<SkillDto> GetById(Guid id);
	Task<bool> DeleteByIds(DeleteRequest request);
    Task<SkillDto> Create(AddSkillRequest request);
    Task<SkillDto> Update(AddSkillRequest request);
}
