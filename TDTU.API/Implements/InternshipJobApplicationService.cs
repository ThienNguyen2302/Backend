using AutoMapper;
using AutoMapper.QueryableExtensions;
using TDTU.API.Dtos.InternshipJobApplicationDTO;
using TDTU.API.Models.InternshipJobApplicationModel;

namespace TDTU.API.Implements;

public class InternshipJobApplicationService : IInternshipJobApplicationService
{
	private readonly IDataContext _context;
	private readonly IMapper _mapper;

	public InternshipJobApplicationService(IDataContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	private async Task<InternshipTerm?> GetCurrentTerm(Guid? id = null, Guid? user = null)
	{
		DateTime now = DateTime.Now;
		InternshipTerm? term = null;
		var query = _context.InternshipTerms
							.Where(s => s.StartDate < now && now < s.EndDate && s.IsExpired != true)
							.AsNoTracking();

		if (id != null && id != Guid.Empty)
		{
			query = query.Where(s => s.Id == id);
		}

		term = await query.FirstOrDefaultAsync();

		if (user != null && user != Guid.Empty && term != null)
		{
			var valid = await _context.InternshipRegistrations
							  .Where(s => s.InternshipTermId == term.Id && s.StudentId == user)
							  .FirstOrDefaultAsync();

			if (valid == null) return null;
		}

		return term;
	}

	private async Task<Company> FindCompany(Guid id)
	{
		var company = await _context.Companies.FirstOrDefaultAsync(s => s.Id == id);
		if (company == null)
		{
			throw new ApplicationException($"Không tìm thấy doanh nghiệp với Id: {id}");
		}
		return company;
	}

	private async Task<ApplicationStatus> FindStatus(string id)
	{
		var status = await _context.ApplicationStatus.FirstOrDefaultAsync(s => s.Id == id);
		if (status == null)
		{
			throw new ApplicationException($"Trạng thái không hợp lệ: {status}");
		}
		return status;
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

	private async Task<InternshipJob> FindJob(Guid id)
	{
		var job = await _context.InternshipJobs.Where(s => s.Company != null && s.InternshipTerm != null)
								.Include(s => s.Company).Include(s => s.InternshipTerm)
								.FirstOrDefaultAsync(s => s.Id == id && s.Company != null);
		if (job == null)
		{
			throw new ApplicationException($"Không tìm thấy công việc với Id: {id}");
		}

		if(job.InternshipTerm!.EndDate < DateTime.Now)
		{
			throw new ApplicationException($"Công việc đã hết hạn ứng tuyển");
		}

		return job;
	}

	public async Task<InternshipJobApplicationDto> Apply(InternshipJobApply request)
	{
		var status = await FindStatus(ApplicationStatusConstant.Pending);
		var job = await FindJob(request.JobId);
		var student = await FindStudent(request.StudentId);
		var term = await GetCurrentTerm(request.TermId, request.StudentId);

		if (term == null) throw new ApplicationException("Đợt thực tập không khả dụng");

		var exist = await _context.InternshipJobApplications
						  .Where(s => s.StudentId == student.Id && s.JobId == request.JobId)
						  .FirstOrDefaultAsync();

		if (exist != null) throw new ApplicationException("Bạn đã ứng tuyển vị trí này");

		var limitSetting = await _context.Settings.Select(s => s.NumberInternshipJob).FirstOrDefaultAsync();
		if(limitSetting == null)
		{
			limitSetting = 10;
        }
		var limit = await _context.InternshipJobApplications
                          .Where(s => s.CreatedDate.Value == DateTime.Now.Date && s.StudentId == student.Id)
                          .CountAsync();

		if(limit > limitSetting) throw new ApplicationException("Bạn đã đạt giới hạn nộp vào ngày hôm nay");

        var application = new InternshipJobApplication()
		{
			Status = status,
			StatusId = status!.Id,
			Job = job,
			JobId = job.Id,
			Student = student,
			StudentId = student.Id,
			Company = job.Company!.Name,
			Position = job.Name,
			Code = request.Code,
			FullName = request.FullName,
			Email = request.Email,
			Phone = request.Phone,
			CV = request.CV,
			Introduce = request.Introduce ?? ""
		};

		_context.InternshipJobApplications.Add(application);
		await _context.SaveChangesAsync();

		return _mapper.Map<InternshipJobApplicationDto>(application);
	}

	public async Task<InternshipJobApplicationDto> SetStatus(InternshipJobSetStatus request)
	{
		var company = await FindCompany(request.CompanyId);
		var status = await FindStatus(request.Status);

		var application = await _context.InternshipJobApplications
								.Include(s => s.Job).Include(s => s.Status)
								.Where(s => s.Id == request.Id && s.Job != null)
								.FirstOrDefaultAsync();

		if (application == null) throw new ApplicationException($"Không tìm thấy dữ liệu với Id: {request.Id}");

		if (application.Job!.CompanyId != company.Id) throw new ApplicationException($"Bạn không đủ quyền thao tác");

		application.Status = status;
		application.StatusId = status.Id;
		application.LastModifiedApplicationUserId = company.Id;
		application.LastModifiedDate = DateTime.Now;

		if(status.Id == ApplicationStatusConstant.Interning)
		{
			var registration = await _context.InternshipRegistrations
									   .Where(s => s.InternshipTermId == application.Job.InternshipTermId
												&& s.StudentId == application.StudentId)
									   .FirstOrDefaultAsync();
			if(registration != null)
			{
				registration.StatusId = RegistrationStatusConstant.Inprogress;
				_context.InternshipRegistrations.Update(registration);
			}
		}

		_context.InternshipJobApplications.Update(application);
		await _context.SaveChangesAsync();

		return _mapper.Map<InternshipJobApplicationDto>(application);
	}

	public async Task<PaginatedList<InternshipJobApplicationDto>> UserHistory(ApplicationPaginationRequest request)
	{
		if (request.UserId == null) throw new ApplicationException("Không tìm thấy Id người dùng");

		var query = _context.InternshipJobApplications.Include(s => s.Status)
							.Where(m => m.DeleteFlag == false && m.StudentId == request.UserId)
							.OrderByDescending(x => x.CreatedDate)
							.ProjectTo<InternshipJobApplicationDto>(_mapper.ConfigurationProvider);

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.FullName.ToLower().Contains(text) ||
									 x.Company!.ToLower().Contains(text) ||
									 x.Position!.ToLower().Contains(text));
		}

		if (!string.IsNullOrEmpty(request.Status))
		{
			query = query.Where(x => x.StatusId == request.Status);
		}

		if (request.CompanyId != null && request.CompanyId != Guid.Empty)
		{
			var ids = await _context.InternshipJobs
									.Where(s => s.CompanyId == request.CompanyId)
									.Select(s => s.Id)
									.ToListAsync();

			query = query.Where(s => ids.Contains(s.JobId));
		}

		PaginatedList<InternshipJobApplicationDto> paging = await query.PaginatedListAsync(request.PageIndex, request.PageSize);
		return paging;
	}

	public async Task<PaginatedList<InternshipJobApplicationDto>> JobApplications(PaginationRequest request, Guid id)
	{
		var query = _context.InternshipJobApplications.Include(s => s.Status)
							.Where(m => m.DeleteFlag == false && m.JobId == id)
							.OrderByDescending(x => x.CreatedDate)
							.ProjectTo<InternshipJobApplicationDto>(_mapper.ConfigurationProvider);

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.FullName.ToLower().Contains(text) ||
									 x.Company!.ToLower().Contains(text) ||
									 x.Position!.ToLower().Contains(text));
		}

		if (!string.IsNullOrEmpty(request.Status))
		{
			query = query.Where(x => x.StatusId == request.Status);
		}

		PaginatedList<InternshipJobApplicationDto> paging = await query.PaginatedListAsync(request.PageIndex, request.PageSize);
		return paging;
	}

	public async Task<InternshipJobApplicationDto> GetById(Guid id)
	{
		var data = await _context.InternshipJobApplications.Include(s => s.Status)
								 .ProjectTo<InternshipJobApplicationDto>(_mapper.ConfigurationProvider)
								 .FirstOrDefaultAsync(s => s.Id == id);
		return data;
	}

	public async Task<bool> DeleteByIds(DeleteRequest request)
	{
		if (request.Ids == null) throw new ApplicationException("Không tìm thấy tham số Id.");
		List<Guid> ids = request.Ids.Select(m => Guid.Parse(m)).ToList();
		var query = await _context.InternshipJobApplications.Where(m => ids.Contains(m.Id)).ToListAsync();
		if (query == null || query.Count == 0) throw new ApplicationException($"Không tìm thấy trong dữ liệu có Id: {string.Join(";", request.Ids)}");

		foreach (var item in query)
		{
			item.DeleteFlag = true;
			item.LastModifiedDate = DateTime.Now;
			item.LastModifiedApplicationUserId = request.ApplicationUserId;
		}
		_context.InternshipJobApplications.UpdateRange(query);

		int rows = await _context.SaveChangesAsync();
		return rows > 0;
	}
}
