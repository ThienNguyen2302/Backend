using AutoMapper;
using AutoMapper.QueryableExtensions;
using TDTU.API.Dtos.OrderStatusDTO;

namespace TDTU.API.Implements;

public class OrderStatusService : IOrderStatusService
{
	private readonly IDataContext _context;
	private readonly IMapper _mapper;
	public OrderStatusService(IDataContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}
	public async Task<List<OrderStatusDto>> GetAll(BaseRequest request)
	{
		List<OrderStatusDto> data = await _context.OrderStatus
										  .ProjectTo<OrderStatusDto>(_mapper.ConfigurationProvider)
										  .ToListAsync();
		return data;
	}

	public async Task<List<OrderStatusDto>> GetFilter(FilterRequest request)
	{
		var query = _context.OrderStatus.OrderBy(s => s.Name)
					.ProjectTo<OrderStatusDto>(_mapper.ConfigurationProvider)
					.AsNoTracking();

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.Name.ToLower().Contains(text) ||
									 x.Description.ToLower().Contains(text));
		}

		if (request.Skip != null)
		{
			query = query.Skip(request.Skip.Value);
		}

		if (request.TotalRecord != null)
		{
			query = query.Take(request.TotalRecord.Value);
		}

		return await query.ToListAsync();
	}

	public async Task<PaginatedList<OrderStatusDto>> GetPagination(PaginationRequest request)
	{
		var query = _context.OrderStatus
					.OrderBy(x => x.Name)
					.ProjectTo<OrderStatusDto>(_mapper.ConfigurationProvider);

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.Name.ToLower().Contains(text) ||
									 x.Description.ToLower().Contains(text));
		}

		PaginatedList<OrderStatusDto> paging = await query.PaginatedListAsync(request.PageIndex, request.PageSize);
		return paging;
	}
}
