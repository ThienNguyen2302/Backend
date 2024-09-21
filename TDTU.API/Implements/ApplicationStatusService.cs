using AutoMapper;
using AutoMapper.QueryableExtensions;
using TDTU.API.Dtos.ApplicationStatusDTO;
namespace TDTU.API.Implements;

public class ApplicationStatusService : IApplicationStatusService
{
	private readonly IDataContext _context;
	private readonly IMapper _mapper;
	public ApplicationStatusService(IDataContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<List<ApplicationStatusDto>> GetAll(BaseRequest request)
	{
		List<ApplicationStatusDto> data = await _context.ApplicationStatus
												.ProjectTo<ApplicationStatusDto>(_mapper.ConfigurationProvider)
												.ToListAsync();
		return data;
	}

	public async Task<List<ApplicationStatusDto>> GetFilter(FilterRequest request)
	{
		var query = _context.ApplicationStatus.OrderBy(s => s.Name)
					.ProjectTo<ApplicationStatusDto>(_mapper.ConfigurationProvider)
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

	public async Task<PaginatedList<ApplicationStatusDto>> GetPagination(PaginationRequest request)
	{
		var query = _context.ApplicationStatus
					.OrderBy(x => x.Name)
					.ProjectTo<ApplicationStatusDto>(_mapper.ConfigurationProvider);

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.Name.ToLower().Contains(text) ||
									 x.Description.ToLower().Contains(text));
		}

		PaginatedList<ApplicationStatusDto> paging = await query.PaginatedListAsync(request.PageIndex, request.PageSize);
		return paging;
	}
}
