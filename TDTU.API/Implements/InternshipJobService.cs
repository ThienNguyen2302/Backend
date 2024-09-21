using AutoMapper;
using AutoMapper.QueryableExtensions;
using TDTU.API.Dtos.InternshipJobDTO;
using TDTU.API.Models.InternshipJobModel;
using TDTU.API.Models.InternshipRegistrationModel;

namespace TDTU.API.Implements;

public class InternshipJobService : IInternshipJobService
{
	private readonly IDataContext _context;
	private readonly IMapper _mapper;
	public InternshipJobService(IDataContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	private async Task<InternshipTerm> FindTerm(Guid id)
	{
		DateTime now = DateTime.Now;
		var term = await _context.InternshipTerms
				   .Where(s => s.StartDate < now && now < s.EndDate &&
							   s.IsExpired != true && s.Id == id)
				   .FirstOrDefaultAsync();

		if (term == null)
		{
			throw new ApplicationException("Đợt thực tập hiện không khả dụng");
		}

		return term;
	}

	private async Task<Company> FindCompany(Guid id)
	{
		var company = await _context.Companies.FirstOrDefaultAsync(s => s.Id == id);
		if (company == null)
		{
			throw new ApplicationException($"Không tìm thấy doanh nghiệp với ID: {id}");
		}

		return company;
	}

	public async Task<InternshipJobDto> Add(InternshipJobAddOrUpdate request)
	{
		var company = await FindCompany(request.CompanyId);
		var term = await FindTerm(request.InternshipTermId);

		var job = new InternshipJob()
		{
			CompanyId = company.Id,
			Company = company,
			InternshipTerm = term,
			InternshipTermId = term.Id,
			Name = request.Name,
			Description = request.Description,
			MaxStudent = request.MaxStudent,
			CreatedApplicationUserId = request.CreatedApplicationUserId,
		};

		if (request.Skills.Any())
		{
			var skills = await _context.Skills.Where(s => request.Skills.Contains(s.Id)).ToListAsync();
			job.Skills = skills;
		}

		_context.InternshipJobs.Add(job);
		await _context.SaveChangesAsync();
		return _mapper.Map<InternshipJobDto>(job);
	}

	public async Task<bool> DeleteByIds(DeleteRequest request)
	{
		if (request.Ids == null) throw new ApplicationException("Không tìm thấy tham số Id.");
		List<Guid> ids = request.Ids.Select(m => Guid.Parse(m)).ToList();
		var query = await _context.InternshipJobs.Where(m => ids.Contains(m.Id)).ToListAsync();
		if (query == null || query.Count == 0) throw new ApplicationException($"Không tìm thấy trong dữ liệu có Id: {string.Join(";", request.Ids)}");

		foreach (var item in query)
		{
			item.DeleteFlag = true;
			item.LastModifiedDate = DateTime.Now;
			item.LastModifiedApplicationUserId = request.ApplicationUserId;
		}
		_context.InternshipJobs.UpdateRange(query);

		int rows = await _context.SaveChangesAsync();
		return rows > 0;
	}

	public async Task<List<InternshipJobDto>> GetAll(BaseRequest request)
	{
		List<InternshipJobDto> data = await _context.InternshipJobs.Where(s => s.Company != null)
											  .OrderByDescending(s => s.CreatedDate)
											  .Include(s => s.Company).ThenInclude(s => s.User)
											  .ProjectTo<InternshipJobDto>(_mapper.ConfigurationProvider)
											  .ToListAsync();
		return data;
	}

	public async Task<InternshipJobDto> GetById(Guid id,Guid? userId = null)
	{
		InternshipJobDto? data = await _context.InternshipJobs.Include(s => s.Skills)
											.Include(s => s.Company).ThenInclude(s => s.User)
											.Where(s => s.Id == id && s.Company != null)
											.ProjectTo<InternshipJobDto>(_mapper.ConfigurationProvider)
											.FirstOrDefaultAsync();

		if(userId != null && data != null)
		{
			data.ApplyStatus = (await  _context.InternshipJobApplications.Include(s => s.Status)
									   .Where(s => s.StudentId == userId && s.JobId == data.Id && s.Status != null)
									   .Select(s => s.Status!.Name)
									   .FirstOrDefaultAsync() ) ?? "";
		}

		return data;
	}

	public async Task<List<InternshipJobDto>> GetFilter(FilterRequest request)
	{
		var query = _context.InternshipJobs.Where(s => s.Company != null)
							.OrderByDescending(s => s.CreatedDate)
							.Include(s => s.Skills)
							.Include(s => s.Company).ThenInclude(s => s.User)
							.ProjectTo<InternshipJobDto>(_mapper.ConfigurationProvider).AsNoTracking();

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.Name.ToLower().Contains(text));
		}

		if (request.UserId != null && request.UserId != Guid.Empty)
		{
			query = query.Where(x => x.CompanyId == request.UserId);
		}

		if (request.TermId != null && request.TermId != Guid.Empty)
		{
			query = query.Where(x => x.InternshipTermId == request.TermId);
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

	public async Task<PaginatedList<InternshipJobDto>> GetPagination(JobPaginationRequest request)
	{
		var query = _context.InternshipJobs.Where(s => s.Company != null)
							.OrderByDescending(s => s.CreatedDate)
							.Include(s => s.Skills)
							.Include(s => s.Company).ThenInclude(s => s.User)
							.OrderByDescending(x => x.CreatedDate)
							.ProjectTo<InternshipJobDto>(_mapper.ConfigurationProvider);

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.Name.ToLower().Contains(text));
		}

		if (!string.IsNullOrEmpty(request.RoleId) && request.RoleId != RoleConstant.Company)
		{
			query = query.Where(x => x.Applications < x.MaxStudent);
		}

		if (request.SkillId != null && request.SkillId != Guid.Empty)
		{
			query = query.Where(x => x.Skills.Any(s => s.Id == request.SkillId));
		}

		if (request.TermId != null && request.TermId != Guid.Empty)
		{
			query = query.Where(x => x.InternshipTermId == request.TermId);
		}

		if (request.UserId != null && request.UserId != Guid.Empty)
		{
			query = query.Where(x => x.CompanyId == request.UserId);
		}

		PaginatedList<InternshipJobDto> paging = await query.PaginatedListAsync(request.PageIndex, request.PageSize);
		return paging;
	}

	private async Task<InternshipJob> FindAsync(Guid? id)
	{
		if (id == null) throw new ApplicationException($"Không tìm thấy dữ liệu với Id");

		var job = await _context.InternshipJobs.Include(s => s.Skills)
								.Include(s => s.Company).ThenInclude(s => s.User)
								.FirstOrDefaultAsync(s => s.Id == id);

		if (job == null) throw new ApplicationException($"Không tìm thấy dữ liệu với Id: {id}");

		return job;
	}

	public async Task<InternshipJobDto> Update(InternshipJobAddOrUpdate request)
	{
		var company = await FindCompany(request.CompanyId);
		var term = await FindTerm(request.InternshipTermId);
		var job = await FindAsync(request.Id);

		if (company.Id != job.CompanyId) throw new ApplicationException($"Bạn không đủ quyền thao tác");

		job.Name = request.Name;
		job.InternshipTerm = term;
		job.InternshipTermId = term.Id;
		job.CompanyId = company.Id;
		job.Company = company;
		job.Description = request.Description;
		job.MaxStudent = request.MaxStudent;

		if (job.Skills != null && job.Skills.Any())
		{
			job.Skills.Clear();
		}
		job.Skills = await _context.Skills.Where(s => request.Skills.Contains(s.Id)).ToListAsync();

		_context.InternshipJobs.Update(job);
		await _context.SaveChangesAsync();

		return _mapper.Map<InternshipJobDto>(job);
	}

	public async Task<PaginatedList<InternshipJobDto>> Suggest(SuggestRequest request)
	{
		if (string.IsNullOrEmpty(request.SkillIds))
		{
			return PaginatedList<InternshipJobDto>.Empty(request.PageIndex);
		}

		var query = _context.InternshipJobs
							.Where(s => s.Company != null && s.InternshipTermId == request.TermId) 
							.OrderByDescending(s => s.CreatedDate)
							.Include(s => s.Skills)
							.Include(s => s.Company).ThenInclude(s => s.User)
							.OrderByDescending(x => x.CreatedDate)
							.ProjectTo<InternshipJobDto>(_mapper.ConfigurationProvider);

		if (request.Id != null && request.Id != Guid.Empty)
		{
			query = query.Where(x => x.Id != request.Id);
		}

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.Name.ToLower().Contains(text));
		}

		if (!string.IsNullOrEmpty(request.SkillIds))
		{
			List<Guid> ids = request.SkillIds.Split(",").Select(m => Guid.Parse(m)).ToList();
			query = query.Where(x => ids.Any(s => x.Skills.Select(m => m.Id).Contains(s)));
		}

		PaginatedList<InternshipJobDto> paging = await query.PaginatedListAsync(request.PageIndex, request.PageSize);
		return paging;
	}

	public async Task<bool> AttachStudent(AttachStudentToJob request)
	{
		var registrations = await _context.InternshipRegistrations
										  .Where(s => request.RegistrationIds.Contains(s.Id))
										  .ToListAsync();

		if (!registrations.Any())
		{
			throw new ApplicationException("Không tìm thấy học sinh");
		}

		var studentIds = registrations.Select(s => s.StudentId).Distinct().ToList();

		var students = await _context.Students.Include(s => s.User).Include(s => s.Profiles)
									.Where(s => studentIds.Contains(s.Id))
									.ToListAsync();

		var job = await _context.InternshipJobs.Include(s => s.Applications).Include(s => s.Company)
								.FirstOrDefaultAsync(s => s.Id == request.JobId);

		if (job == null)
		{
			throw new ApplicationException("Không tìm thấy công việc thực tập");
		}

		int applications = job.Applications == null ? 0 :
			job.Applications.Where(s => s.StatusId == ApplicationStatusConstant.Interning).Count();

		if(job.MaxStudent <= applications)
		{
			throw new ApplicationException("Công việc đã nhận đủ sinh viên");
		}
		else
		{
			int remainingSlots = job.MaxStudent - applications;

			if (studentIds.Count > remainingSlots)
			{
				throw new ApplicationException($"Công việc chỉ có thể nhận thêm {remainingSlots} học sinh");
			}
		}

		foreach(var student in students)
		{
			var cv = student.Profiles == null ? "" :
				student.Profiles.OrderByDescending(s => s.CreatedDate)
								.Select(s => s.Url)
								.FirstOrDefault();

			var apply = new InternshipJobApplication()
			{
				JobId = job.Id,
				StudentId = student.Id,
				Email = student.User.Email,
				Code = student.Code,
				FullName = student.FullName,
				Introduce = student.Introduction,
				Phone = student.User.Phone,
				Position = job.Name,
				StatusId = ApplicationStatusConstant.Pending,
				Company = job.Company != null ? job.Company.Name : "",
				CV = cv ?? ""
			};
			_context.InternshipJobApplications.Add(apply);
		}
		
		/*foreach(var registration in registrations)
		{
			registration.StatusId = RegistrationStatusConstant.Inprogress;
			_context.InternshipRegistrations.Update(registration);

			var order = new InternshipOrder()
			{
				StatusId = OrderStatusConstant.Accepted,
				RegistrationId = registration.Id,
				Registration = registration,
				Position = job.Name,
				Company = job.Company != null ? job.Company.Name : "",
				StartDate = DateTime.Now,
				EndDate = DateTime.Now.AddMonths(3),
				TaxCode = job.Company != null ? job.Company.TaxCode : "",
				IsAttach = true,
				CreatedDate = DateTime.Now,
				LastModifiedDate = DateTime.Now
			};

			_context.InternshipOrders.Add(order);
		}*/

		await _context.SaveChangesAsync();

		return true;
	}
}