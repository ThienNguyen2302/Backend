using AutoMapper;
using TDTU.API.Dtos.InternshipRegistrationDTO;
using TDTU.API.Dtos.RegistrationStatusDTO;
using TDTU.API.Models.InternshipRegistrationModel;

namespace TDTU.API.Implements;

public class InternshipRegistrationService : IInternshipRegistrationService
{
	private readonly IDataContext _context;
	private readonly IMapper _mapper;
	public InternshipRegistrationService(IDataContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
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

	private async Task<InternshipTerm> FindTerm(Guid id)
	{
		DateTime now = DateTime.Now;
		var term = await _context.InternshipTerms
						 .Where(s => s.StartDate < now && now < s.EndDate &&
									 s.IsExpired != true && s.Id == id)
						 .FirstOrDefaultAsync();

		if (term == null)
		{
			throw new ApplicationException($"Không tìm thấy học sinh với Id: {id}");
		}

		return term;
	}

	public async Task<InternshipRegistrationDto> Add(InternshipRegistrationAddOrUpdate request)
	{
		var term = await FindTerm(request.TermId);
		var student = await _context.Students.FirstOrDefaultAsync(s => s.Code == request.Code);
		if (student == null)
		{
			var user = new User()
			{
				Email = StringHelper.BoDauVaKhoangTrang(request.FullName) + "@gmail.com",
				Password = "123456",
				Phone = "",
				Address = "",
				RoleId = RoleConstant.Student
			};
			_context.Users.Add(user);

			student = new Student()
			{
				Id = user.Id,
				FullName = request.FullName,
				Code = request.Code,
				StartDate = DateTime.Now,
				Major = "",
			};
			_context.Students.Add(student);

			int row = await _context.SaveChangesAsync();

			if (row == 0) throw new ApplicationException("Tạo học sinh thất bại");
		}
		else
		{
			var exist = await _context.InternshipRegistrations
						.Where(s => s.StudentId == student.Id && s.InternshipTermId == term.Id)
						.FirstOrDefaultAsync();

			if (exist != null) throw new ApplicationException("Học sinh đã đăng kí đợt thực tập hiện tại");
		}

		var status = await _context.RegistrationStatus.FindAsync(RegistrationStatusConstant.Pending);

		if (status == null) throw new ApplicationException("Trạng thái không hợp lệ");

		var registration = new InternshipRegistration()
		{
			StatusId = status.Id,
			Status = status,
			InternshipTermId = term.Id,
			InternshipTerm = term,
			StudentId = student.Id,
			Student = student,
			IsExpired = false,
			CreatedApplicationUserId = request.CreatedApplicationUserId,
			LastModifiedApplicationUserId = request.LastModifiedApplicationUserId
		};

		_context.InternshipRegistrations.Add(registration);
		await _context.SaveChangesAsync();

		return await GetById(registration.Id);
	}

	public async Task<bool> DeleteByIds(DeleteRequest request)
	{
		if (request.Ids == null) throw new ApplicationException("Không tìm thấy tham số Id.");
		List<Guid> ids = request.Ids.Select(m => Guid.Parse(m)).ToList();
		var query = await _context.InternshipRegistrations.Include(s => s.Orders).Where(m => ids.Contains(m.Id)).ToListAsync();
		if (query == null || query.Count == 0) throw new ApplicationException($"Không tìm thấy trong dữ liệu có Id: {string.Join(";", request.Ids)}");

		foreach (var item in query)
		{
			item.DeleteFlag = true;
			item.LastModifiedDate = DateTime.Now;
			item.LastModifiedApplicationUserId = request.ApplicationUserId;

			if(item.Orders != null && item.Orders.Any())
			{
				DeleteOrders(item.Orders);
				_context.InternshipOrders.UpdateRange(item.Orders);
			}
		}
		_context.InternshipRegistrations.UpdateRange(query);

		int rows = await _context.SaveChangesAsync();
		return rows > 0;
	}

	private void DeleteOrders(ICollection<InternshipOrder> orders)
	{
		foreach (var order in orders)
		{
			order.DeleteFlag = true;
			order.LastModifiedDate = DateTime.Now;
		}
	}

	public async Task<PaginatedList<InternshipRegistrationDto>> GetPagination(PaginationRequest request)
	{
		var empty = new RegistrationJobDto();
		var query = _context.InternshipRegistrations
							.OrderByDescending(s => s.CreatedDate)
							.Include(s => s.Orders!)
							.Include(s => s.Status)
							.Include(s => s.Student)
							.Where(s => s.Student != null && s.Status != null)
							.Select(s => new InternshipRegistrationDto()
							{
								Id = s.Id,
								StudentId = s.StudentId,
								InternshipTermId = s.InternshipTermId,
								StatusId = s.StatusId,
								Code = s.Student!.Code,
								FullName = s.Student!.FullName,
								Status = new RegistrationStatusDto()
								{
									Id = s.Status!.Id,
									Name = s.Status.Name,
									Description = s.Status.Description
								},
								Job = s.Orders != null ?
									  s.Orders
									   .Where(x => x.StatusId == OrderStatusConstant.Accepted)
									   .Select(x => new RegistrationJobDto()
									   {
										   Id = x.Id,
										   Position = x.Position,
										   Company = x.Company,
										   StatusId = x.StatusId ?? ""
									   }).FirstOrDefault() ?? empty : empty
							})
							.AsNoTracking();

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.FullName.ToLower().Contains(text) ||
									 x.Code.ToLower().Contains(text));
		}

		if (!string.IsNullOrEmpty(request.Status))
		{
			query = query.Where(x => x.StatusId == request.Status);
		}

		if (!string.IsNullOrEmpty(request.Status))
		{
			query = query.Where(x => x.StatusId == request.Status);
		}

		if (request.TermId != null && request.TermId != Guid.Empty)
		{
			query = query.Where(x => x.InternshipTermId == request.TermId);
		}


		PaginatedList<InternshipRegistrationDto> paging = await query.PaginatedListAsync(request.PageIndex, request.PageSize);
		return paging;
	}

	public async Task<InternshipRegistrationDto> Update(InternshipRegistrationAddOrUpdate request)
	{
		var student = await FindStudent(request.StudentId);
		var term = await FindTerm(request.TermId);

		var registration = await _context.InternshipRegistrations
								 .Where(s => s.StudentId == student.Id && s.InternshipTermId == term.Id)
								 .FirstOrDefaultAsync();

		if (registration == null) throw new ApplicationException("Học sinh không tồn tại trong kì thực tập");

		if (registration.IsExpired == true) throw new ApplicationException("Kì thực tập đã hết hạn không thể chỉnh sửa");

		student.Code = request.Code;
		student.FullName = request.FullName;
		student.LastModifiedApplicationUserId = request.LastModifiedApplicationUserId;
		student.LastModifiedDate = DateTime.Now;

		_context.Students.Update(student);
		await _context.SaveChangesAsync();

		return await GetById(registration.Id);
	}

	public async Task<InternshipRegistrationDto> GetById(Guid id)
	{
		var empty = new RegistrationJobDto();
		var data = await _context.InternshipRegistrations
						 .Include(s => s.Orders!)
						 .Include(s => s.Status)
						 .Include(s => s.Student)
						 .Where(s => s.Id == id && s.Student != null && s.Status != null)
						 .Select(s => new InternshipRegistrationDto()
						 {
							 Id = s.Id,
							 StudentId = s.StudentId,
							 InternshipTermId = s.InternshipTermId,
							 StatusId = s.StatusId,
							 Code = s.Student!.Code,
							 FullName = s.Student!.FullName,
							 Status = new RegistrationStatusDto()
							 {
								 Id = s.Status!.Id,
								 Name = s.Status.Name,
								 Description = s.Status.Description
							 },
							 Job = s.Orders != null ?
								   s.Orders
									.Where(x => x.StatusId == OrderStatusConstant.Accepted)
									.Select(x => new RegistrationJobDto()
									{
										Id = x.Id,
										Position = x.Position,
										Company = x.Company,
										StatusId = x.StatusId ?? ""
									}).FirstOrDefault() ?? empty : empty
						 })
						 .FirstOrDefaultAsync();
		return data;
	}

	public async Task<PaginatedList<InternshipRegistrationDto>> GetPendingStudent(BaseRequest request)
	{
		var empty = new RegistrationJobDto();
		var data = await _context.InternshipRegistrations
						 .Include(s => s.Orders!)
						 .Include(s => s.Status)
						 .Include(s => s.Student)
						 .Where(s => s.Student != null && s.Status != null 
								  && s.StatusId == RegistrationStatusConstant.Pending)
						 .Select(s => new InternshipRegistrationDto()
						 {
							 Id = s.Id,
							 StudentId = s.StudentId,
							 InternshipTermId = s.InternshipTermId,
							 StatusId = s.StatusId,
							 Code = s.Student!.Code,
							 FullName = s.Student!.FullName,
							 Status = new RegistrationStatusDto()
							 {
								 Id = s.Status!.Id,
								 Name = s.Status.Name,
								 Description = s.Status.Description
							 },
							 Job = s.Orders != null ?
								   s.Orders
									.Where(x => x.StatusId == OrderStatusConstant.Accepted)
									.Select(x => new RegistrationJobDto()
									{
										Id = x.Id,
										Position = x.Position,
										Company = x.Company,
										StatusId = x.StatusId ?? ""
									}).FirstOrDefault() ?? empty : empty
						 })
						 .ToListAsync();

		return PaginatedList<InternshipRegistrationDto>.ConvertArray(data);
	}
}
