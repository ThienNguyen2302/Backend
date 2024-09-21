using AutoMapper;
using AutoMapper.QueryableExtensions;
using TDTU.API.Dtos.SkillDTO;
using TDTU.API.Models.SkillModel;

namespace TDTU.API.Implements;

public class SkillService : ISkillService
{
	private readonly IDataContext _context;
	private readonly IMapper _mapper;
	public SkillService(IDataContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

    public async Task<SkillDto> Create(AddSkillRequest request)
    {
        if (request.Name == null) throw new ApplicationException("Tên của kỹ năng không thể rỗng");

        if (request.Description == null) throw new ApplicationException("Chú thích của kỹ năng không thể rỗng");

		var lastSkill = await _context.Skills.OrderByDescending(s => s.CreatedDate).LastOrDefaultAsync();

		Skill skill = new Skill();
		skill.Name = request.Name;
		skill.Description = request.Description;
		skill.Sort = "1";
		skill.CreatedDate = DateTime.Now;
		skill.LastModifiedDate = DateTime.Now;
		skill.CreatedApplicationUserId = request.ApplicationUserId;
		skill.LastModifiedApplicationUserId = request.ApplicationUserId;

		if (lastSkill != null)
		{
			skill.Sort = (int.Parse(lastSkill.Sort) + 1).ToString();
		}

		_context.Skills.Add(skill);
        await _context.SaveChangesAsync();

        return _mapper.Map<SkillDto>(skill);
    }

    public async Task<bool> DeleteByIds(DeleteRequest request)
	{
		if (request.Ids == null) throw new ApplicationException("Không tìm thấy tham số Id.");
		List<Guid> ids = request.Ids.Select(m => Guid.Parse(m)).ToList();
		var query = await _context.Skills.Where(m => ids.Contains(m.Id)).ToListAsync();
		if (query == null || query.Count == 0) throw new ApplicationException($"Không tìm thấy trong dữ liệu có Id: {string.Join(";", request.Ids)}");

		foreach (var item in query)
		{
			item.DeleteFlag = true;
			item.LastModifiedDate = DateTime.Now;
			item.LastModifiedApplicationUserId = request.ApplicationUserId;
		}
		_context.Skills.UpdateRange(query);

		int rows = await _context.SaveChangesAsync();
		return rows > 0;
	}

	public async Task<List<SkillDto>> GetAll(BaseRequest request)
	{
		List<SkillDto> data = await _context.Skills
									.OrderByDescending(s => s.CreatedDate)
									.ProjectTo<SkillDto>(_mapper.ConfigurationProvider)
									.ToListAsync();
		return data;
	}

	public async Task<SkillDto> GetById(Guid id)
	{
		SkillDto? data = await _context.Skills
									   .ProjectTo<SkillDto>(_mapper.ConfigurationProvider)
									   .FirstOrDefaultAsync();
		return data;
	}

	public async Task<List<SkillDto>> GetFilter(FilterRequest request)
	{
		var query = _context.Skills
					.OrderByDescending(s => s.CreatedDate)
					.ProjectTo<SkillDto>(_mapper.ConfigurationProvider)
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

	public async Task<PaginatedList<SkillDto>> GetPagination(PaginationRequest request)
	{
		var query = _context.Skills
					.OrderByDescending(s => s.CreatedDate)
					.Where(m => m.DeleteFlag == false)
				    .OrderByDescending(x => x.Name)
					.ProjectTo<SkillDto>(_mapper.ConfigurationProvider);

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.Name.ToLower().Contains(text) ||
									 x.Description.ToLower().Contains(text));
		}

		PaginatedList<SkillDto> paging = await query.PaginatedListAsync(request.PageIndex, request.PageSize);
		return paging;
	}

	public async Task<SkillDto> Update(AddSkillRequest request)
	{
		if (request.Id == null) throw new ApplicationException("Không tìm thấy dữ liệu");
		
		if (request.Name == null) throw new ApplicationException("Tên của kỹ năng không thể rỗng");

		if (request.Description == null) throw new ApplicationException("Chú thích của kỹ năng không thể rỗng");

		Skill? skill = await _context.Skills.FindAsync(request.Id);

		if (skill == null) throw new ApplicationException("Không tìm thấy dữ liệu");

		skill.Name = request.Name;
		skill.Description = request.Description;
		skill.LastModifiedDate = DateTime.Now;
		skill.LastModifiedApplicationUserId = request.ApplicationUserId;

		_context.Skills.Update(skill);
		await _context.SaveChangesAsync();

		return _mapper.Map<SkillDto>(skill);
	}
}
