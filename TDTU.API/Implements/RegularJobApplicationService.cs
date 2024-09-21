using AutoMapper;
using AutoMapper.QueryableExtensions;
using TDTU.API.Dtos.RegularJobApplicationDTO;
using TDTU.API.Models.RegularJobApplicationModel;

namespace TDTU.API.Implements;

public class RegularJobApplicationService : IRegularJobApplicationService
{
	private readonly IDataContext _context;
	private readonly IMapper _mapper;
	public RegularJobApplicationService(IDataContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	private async Task<RegularJob> FindJob(Guid id,bool? checkExpire = false)
	{
		var job = await _context.RegularJobs.Include(s => s.Company)
								.FirstOrDefaultAsync(s => s.Id == id);
		if(job == null)
		{
			throw new ApplicationException($"Không tìm thấy cộng việc với Id: {id}");
		}
		if(checkExpire == true && job.ExpireDate < DateTime.Now)
		{
			throw new ApplicationException($"Công việc ứng tuyển đã hết hạn");
		}
		return job;
	}

	private async Task<Student> FindStudent(Guid id)
	{
		var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
		if (student == null)
		{
			throw new ApplicationException($"Không tìm thấy học sinh với Id: {id}");
		}
		return student;
	}

	private async Task<bool> IsApply(Guid userId, Guid jobId)
	{
		var exist = await _context.RegularJobApplications
						  .Where(s => s.StudentId == userId && s.JobId == jobId)
						  .FirstOrDefaultAsync();

		return exist != null;
	}

	public async Task<RegularJobApplicationDto> ApplyJob(RegularJobApplyRequest request)
	{
		var job = await FindJob(request.JobId, true);
		var student = await FindStudent(request.StudentId);
		var isApply = await IsApply(student.Id, job.Id);
		if (isApply == true) throw new ApplicationException("Bạn đã ứng tuyển vào vị trí này");

		var application = new RegularJobApplication()
		{
			Student = student,
			StudentId = student.Id,
			Job = job,
			JobId = job.Id,
			Company = job.Company != null ? job.Company.Name : "",
			Position = job.Name,
			SalaryMin = job.SalaryMin,
			SalaryMax = job.SalaryMax,
			FullName = request.FullName,
			Email = request.Email,
			Phone = request.Phone,
			CV = request.CV,
			Introduce = request.Introduce ?? ""
		};
		_context.RegularJobApplications.Add(application);
		await _context.SaveChangesAsync();

		return _mapper.Map<RegularJobApplicationDto>(application);
	}

	public async Task<bool> DeleteByIds(DeleteRequest request)
	{
		if (request.Ids == null) throw new ApplicationException("Không tìm thấy tham số Id.");
		List<Guid> ids = request.Ids.Select(m => Guid.Parse(m)).ToList();
		var query = await _context.RegularJobApplications.Where(m => ids.Contains(m.Id)).ToListAsync();
		if (query == null || query.Count == 0) throw new ApplicationException($"Không tìm thấy trong dữ liệu có Id: {string.Join(";", request.Ids)}");

		foreach (var item in query)
		{
			item.DeleteFlag = true;
			item.LastModifiedDate = DateTime.Now;
			item.LastModifiedApplicationUserId = request.ApplicationUserId;
		}
		_context.RegularJobApplications.UpdateRange(query);

		int rows = await _context.SaveChangesAsync();
		return rows > 0;
	}

	public async Task<RegularJobApplicationDto> GetById(Guid id)
	{
		var data = await _context.RegularJobApplications
								 .ProjectTo<RegularJobApplicationDto>(_mapper.ConfigurationProvider)
								 .FirstOrDefaultAsync(s => s.Id == id);
		return data;
	}

	public async Task<PaginatedList<RegularJobApplicationDto>> JobApplications(PaginationRequest request, Guid id)
	{
		var query = _context.RegularJobApplications
							.OrderByDescending(s => s.CreatedDate)
							.Include(s => s.Job)
							.Where(m => m.DeleteFlag == false && m.JobId == id)
							.OrderByDescending(x => x.CreatedDate)
							.ProjectTo<RegularJobApplicationDto>(_mapper.ConfigurationProvider);

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.FullName.ToLower().Contains(text) ||
									 x.Company.ToLower().Contains(text) ||
									 x.Position.ToLower().Contains(text));
		}

		PaginatedList<RegularJobApplicationDto> paging = await query.PaginatedListAsync(request.PageIndex, request.PageSize);
		return paging;
	}

	public async Task<PaginatedList<RegularJobApplicationDto>> UserHistory(ApplicationPaginationRequest request)
	{
		if (request.UserId == null) throw new ApplicationException("Không tìm thấy Id doanh nghiệp");

		var query = _context.RegularJobApplications
							.OrderByDescending(s => s.CreatedDate)
							.Include(s => s.Job)
							.Where(m => m.DeleteFlag == false && m.StudentId == request.UserId)
							.OrderByDescending(x => x.CreatedDate)
							.ProjectTo<RegularJobApplicationDto>(_mapper.ConfigurationProvider);

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.FullName.ToLower().Contains(text) ||
									 x.Company.ToLower().Contains(text) ||
									 x.Position.ToLower().Contains(text));
		}

		if(request.CompanyId != null && request.CompanyId != Guid.Empty)
		{
			var ids = await _context.RegularJobs
									.Where(s => s.CompanyId == request.CompanyId)
									.Select(s => s.Id)
									.ToListAsync();

			query = query.Where(s => ids.Contains(s.JobId));
		}

		PaginatedList<RegularJobApplicationDto> paging = await query.PaginatedListAsync(request.PageIndex, request.PageSize);
		return paging;
	}
}
