using TDTU.API.Dtos.CompanyDTO;
using TDTU.API.Models.CompanyModel;

namespace TDTU.API.Interfaces;

public interface ICompanyService
{
	Task<PaginatedList<CompanyDto>> GetPagination(PaginationRequest request);
	Task<List<CompanyDto>> GetAll(BaseRequest request);
	Task<List<CompanyDto>> GetFilter(FilterRequest request);
	Task<CompanyDto> GetById(Guid id);
	Task<bool> DeleteByIds(DeleteRequest request);
	Task<CompanyDto> Add(CompanyAddOrUpdate request);
	Task<CompanyDto> Update(CompanyAddOrUpdate request);
}
