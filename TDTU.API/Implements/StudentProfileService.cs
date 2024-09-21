using AutoMapper;
using AutoMapper.QueryableExtensions;
using TDTU.API.Dtos.StudentDTO;
using TDTU.API.Models.StudentProfileModel;

namespace TDTU.API.Implements;

public class StudentProfileService : IStudentProfileService
{
	private readonly IStorageService _storage;
	private readonly IDataContext _context;
	private readonly IMapper _mapper;
	public StudentProfileService(IDataContext context, IMapper mapper, IStorageService storage)
	{
		_context = context;
		_mapper = mapper;
		_storage = storage;
	}
	public async Task<StudentProfileDto> Add(IFormFile file, Guid id)
	{
		var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
		if (student == null) throw new ApplicationException($"Không tìm thấy dữ liệu với Id: {id}");

		var media = await _storage.Upload(new List<IFormFile> { file });
		if (!media.Any()) throw new ApplicationException($"Đã có lỗi xảy ra");

		StudentProfile profile = new StudentProfile()
		{
			Student = student,
			StudentId = student.Id,
			Name = media[0].OriginalName,
			Url = media[0].Url
		};
		_context.StudentProfiles.Add(profile);
		await _context.SaveChangesAsync();

		return _mapper.Map<StudentProfileDto>(profile);
	}

	public async Task<bool> DeleteByIds(DeleteRequest request)
	{
		if (request.Ids == null) throw new ApplicationException("Không tìm thấy tham số Id.");
		List<Guid> ids = request.Ids.Select(m => Guid.Parse(m)).ToList();
		var query = await _context.StudentProfiles.Where(m => ids.Contains(m.Id)).ToListAsync();
		if (query == null || query.Count == 0) throw new ApplicationException($"Không tìm thấy trong dữ liệu có Id: {string.Join(";", request.Ids)}");

		foreach (var item in query)
		{
			item.DeleteFlag = true;
			item.LastModifiedDate = DateTime.Now;
			item.LastModifiedApplicationUserId = request.ApplicationUserId;
		}
		_context.StudentProfiles.UpdateRange(query);

		int rows = await _context.SaveChangesAsync();
		return rows > 0;
	}

	public async Task<List<StudentProfileDto>> GetFilter(FilterRequest request)
	{
		var query = _context.StudentProfiles
					.OrderByDescending(s => s.CreatedDate)
					.ProjectTo<StudentProfileDto>(_mapper.ConfigurationProvider)
					.AsNoTracking();

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.Name.ToLower().Contains(text));
		}

		if (request.UserId != null && request.UserId != Guid.Empty)
		{
			query = query.Where(x => x.StudentId == request.UserId);
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

	public async Task<PaginatedList<StudentProfileDto>> GetPagination(PaginationRequest request)
	{
		var query = _context.StudentProfiles
							.OrderByDescending(x => x.CreatedDate)
							.ProjectTo<StudentProfileDto>(_mapper.ConfigurationProvider);

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.Name.ToLower().Contains(text));
		}

		if (request.UserId != null && request.UserId != Guid.Empty)
		{
			query = query.Where(x => x.StudentId == request.UserId);
		}

		PaginatedList<StudentProfileDto> paging = await query.PaginatedListAsync(request.PageIndex, request.PageSize);
		return paging;
	}
}
