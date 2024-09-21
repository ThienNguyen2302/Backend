using AutoMapper;
using AutoMapper.QueryableExtensions;
using TDTU.API.Dtos.InternshipTermDTO;
using TDTU.API.Dtos.RegularJobDTO;
using TDTU.API.Models.InternshipTermModel;

namespace TDTU.API.Implements;

public class InternshipTermService : IInternshipTermService
{
	private readonly IDataContext _context;
	private readonly IMapper _mapper;
	public InternshipTermService(IDataContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<InternshipTermDto> Add(InternshipTermAddOrUpdate request)
	{
		DateTime now = DateTime.Now;

		var currentTerm = await _context.InternshipTerms
								.Where(s => s.IsExpired != true && s.StartDate < now && now < s.EndDate)
								.FirstOrDefaultAsync();

		if (currentTerm != null) throw new ApplicationException("Hiện đang trong kì thực tập, không thể tạo thêm");

		if (request.StartDate >= request.EndDate) throw new ApplicationException("Thời gian thực tập không hợp lệ");

		var term = new InternshipTerm()
		{
			Name = request.Name,
			StartDate = request.StartDate,
			EndDate = request.EndDate,
			CreatedApplicationUserId = request.CreatedApplicationUserId,
			LastModifiedApplicationUserId = request.LastModifiedApplicationUserId,
			CreatedDate = DateTime.Now,
			LastModifiedDate = DateTime.Now
		};

		_context.InternshipTerms.Add(term);
		await _context.SaveChangesAsync();

		return _mapper.Map<InternshipTermDto>(term);
	}

	public async Task<bool> DeleteByIds(DeleteRequest request)
	{
		if (request.Ids == null) throw new ApplicationException("Không tìm thấy tham số Id.");
		List<Guid> ids = request.Ids.Select(m => Guid.Parse(m)).ToList();
		var query = await _context.InternshipTerms.Where(m => ids.Contains(m.Id)).ToListAsync();
		if (query == null || query.Count == 0) throw new ApplicationException($"Không tìm thấy trong dữ liệu có Id: {string.Join(";", request.Ids)}");

		foreach (var item in query)
		{
			item.DeleteFlag = true;
			item.LastModifiedDate = DateTime.Now;
			item.LastModifiedApplicationUserId = request.ApplicationUserId;
		}
		_context.InternshipTerms.UpdateRange(query);

		int rows = await _context.SaveChangesAsync();
		return rows > 0;
	}

	public async Task<List<InternshipTermDto>> GetAll(BaseRequest request)
	{
		List<InternshipTermDto> data = await _context.InternshipTerms.OrderByDescending(s => s.CreatedDate)
											 .ProjectTo<InternshipTermDto>(_mapper.ConfigurationProvider)
											 .ToListAsync();
		return data;
	}

	public async Task<InternshipTermDto> GetById(Guid id)
	{
		InternshipTermDto? data = await _context.RegularJobs
										.ProjectTo<InternshipTermDto>(_mapper.ConfigurationProvider)
										.FirstOrDefaultAsync();
		return data;
	}

	public async Task<List<InternshipTermDto>> GetFilter(FilterRequest request)
	{
		var query = _context.InternshipTerms.OrderByDescending(s => s.CreatedDate)
							.ProjectTo<InternshipTermDto>(_mapper.ConfigurationProvider).AsNoTracking();

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.Name.ToLower().Contains(text));
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

	public async Task<PaginatedList<InternshipTermDto>> GetPagination(PaginationRequest request)
	{
		var query = _context.InternshipTerms.OrderByDescending(x => x.CreatedDate)
							.ProjectTo<InternshipTermDto>(_mapper.ConfigurationProvider);

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.Name.ToLower().Contains(text));
		}

		PaginatedList<InternshipTermDto> paging = await query.PaginatedListAsync(request.PageIndex, request.PageSize);
		return paging;
	}

	public async Task<InternshipTermDto> Update(InternshipTermAddOrUpdate request)
	{
		if (request.Id == null) throw new ApplicationException("Không tìm thấy kì thực tập");

		var term = await _context.InternshipTerms.FirstOrDefaultAsync(s => s.Id == request.Id);

		if (term == null) throw new ApplicationException($"Không tìm thấy kì thực tập với Id: {request.Id}");

		if (request.StartDate < request.EndDate) throw new ApplicationException("Thời gian thực tập không hợp lệ");

		term.Name = request.Name;
		term.StartDate = request.StartDate;
		term.EndDate = request.EndDate;
		term.LastModifiedApplicationUserId = request.LastModifiedApplicationUserId;
		term.LastModifiedDate = DateTime.Now;

		_context.InternshipTerms.Update(term);
		await _context.SaveChangesAsync();

		return _mapper.Map<InternshipTermDto>(term);
	}
}
