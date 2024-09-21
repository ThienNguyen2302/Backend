using TDTU.API.Dtos.OrderStatusDTO;

namespace TDTU.API.Interfaces;

public interface IOrderStatusService
{
	Task<PaginatedList<OrderStatusDto>> GetPagination(PaginationRequest request);
	Task<List<OrderStatusDto>> GetAll(BaseRequest request);
	Task<List<OrderStatusDto>> GetFilter(FilterRequest request);
}
