using AutoMapper;
using AutoMapper.QueryableExtensions;
using TDTU.API.Dtos.RegistrationStatusDTO;

namespace TDTU.API.Implements;

public class RegistrationStatusService : IRegistrationStatusService
{
	private readonly IDataContext _context;
	private readonly IMapper _mapper;
	public RegistrationStatusService(IDataContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}
	public async Task<List<RegistrationStatusDto>> GetAll(BaseRequest request)
	{
		List<RegistrationStatusDto> data = await _context.RegistrationStatus
										  .ProjectTo<RegistrationStatusDto>(_mapper.ConfigurationProvider)
										  .ToListAsync();
		return data;
	}

	public async Task<List<RegistrationStatusDto>> GetFilter(FilterRequest request)
	{
		var query = _context.RegistrationStatus.OrderBy(s => s.Name)
					.ProjectTo<RegistrationStatusDto>(_mapper.ConfigurationProvider)
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

	public async Task<PaginatedList<RegistrationStatusDto>> GetPagination(PaginationRequest request)
	{
		var query = _context.RegistrationStatus
					.OrderBy(x => x.Name)
					.ProjectTo<RegistrationStatusDto>(_mapper.ConfigurationProvider);

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.Name.ToLower().Contains(text) ||
									 x.Description.ToLower().Contains(text));
		}

		PaginatedList<RegistrationStatusDto> paging = await query.PaginatedListAsync(request.PageIndex, request.PageSize);
		return paging;
	}
}
