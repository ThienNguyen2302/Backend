using TDTU.API.Dtos.InternshipOrderDTO;
using TDTU.API.Models.InternshipOrderModel;

namespace TDTU.API.Interfaces;

public interface IInternshipOrderService
{
	Task<PaginatedList<InternshipOrderDto>> GetPagination(PaginationRequest request);
	Task<InternshipOrderDto> GetById(Guid id);
	Task<bool> DeleteByIds(DeleteRequest request);
	Task<InternshipOrderDto> Add(InternshipOrderAddOrUpdate request);
	Task<InternshipOrderDto> Update(InternshipOrderAddOrUpdate request);
	Task<InternshipOrderDto> UpdateStatus(InternshipOderUpdateStatus request);
}
