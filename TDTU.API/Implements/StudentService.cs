using AutoMapper;
using AutoMapper.QueryableExtensions;
using CloudinaryDotNet.Core;
using OfficeOpenXml;
using TDTU.API.Dtos.SkillDTO;
using TDTU.API.Dtos.StudentDTO;
using TDTU.API.Models.StudentModel;

namespace TDTU.API.Implements;

public class StudentService : IStudentService
{
	private readonly IDataContext _context;
	private readonly IMapper _mapper;
	public StudentService(IDataContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}
	public async Task<bool> DeleteByIds(DeleteRequest request)
	{
		if (request.Ids == null) throw new ApplicationException("Không tìm thấy tham số Id.");
		List<Guid> ids = request.Ids.Select(m => Guid.Parse(m)).ToList();
		var query = await _context.Students.Include(s => s.User).Where(m => ids.Contains(m.Id)).ToListAsync();
		if (query == null || query.Count == 0) throw new ApplicationException($"Không tìm thấy trong dữ liệu có Id: {string.Join(";", request.Ids)}");

		foreach (var item in query)
		{
			item.User.DeleteFlag = true;
			item.DeleteFlag = true;
			item.LastModifiedDate = DateTime.Now;
			item.LastModifiedApplicationUserId = request.ApplicationUserId;
		}
		_context.Students.UpdateRange(query);

		int rows = await _context.SaveChangesAsync();
		return rows > 0;
	}

	public async Task<List<StudentDto>> GetAll(BaseRequest request)
	{
		List<StudentDto> data = await _context.Students
									  .OrderByDescending(s => s.CreatedDate)
									  .Include(s => s.User)
									  .ProjectTo<StudentDto>(_mapper.ConfigurationProvider)
									  .ToListAsync();
		return data;
	}

	public async Task<StudentDto?> GetById(Guid id)
	{
		StudentDto? data = await _context.Students
								.Where(s => s.Id == id)
								.Include(s => s.User)
								.Include(s => s.StudentSkills!).ThenInclude(ss => ss.Skill)
								.Select(s => new StudentDto
								{
									Id = s.Id,
									Code = s.Code,
									Address = s.User.Address,
									Email = s.User.Email,
									FullName = s.FullName,
									Major = s.Major,
									Phone = s.User.Phone,
									StartDate = s.StartDate,
									Introduction = s.Introduction,
									Skills = s.StudentSkills != null ? s.StudentSkills.Select(x => new SkillDto
									{
										Id = x.Skill!.Id,
										Name = x.Skill.Name,
										Description = x.Skill.Description,
										Sort = x.Skill.Sort
									}).ToList() : new List<SkillDto>()
								})
								.FirstOrDefaultAsync();

		return data;
	}



	public async Task<List<StudentDto>> GetFilter(FilterRequest request)
	{
		var query = _context.Students
					.OrderByDescending(s => s.CreatedDate)
					.ProjectTo<StudentDto>(_mapper.ConfigurationProvider)
					.AsNoTracking();

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.Phone.ToLower().Contains(text) ||
									 x.Email.ToLower().Contains(text) ||
									 x.FullName.ToLower().Contains(text));
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

	public async Task<PaginatedList<StudentDto>> GetPagination(PaginationRequest request)
	{
		var query = _context.Students
					.OrderByDescending(s => s.CreatedDate)
					.Include(s => s.User)
					.ProjectTo<StudentDto>(_mapper.ConfigurationProvider);

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.Email.ToLower().Contains(text) ||
									 x.Phone.ToLower().Contains(text));
		}

		PaginatedList<StudentDto> paging = await query.PaginatedListAsync(request.PageIndex, request.PageSize);
		return paging;
	}

	public async Task<bool> ImportStudent(ImportStudentRequest request)
	{
		if (request.IntershipTermId == Guid.Empty)
			throw new Exception("Không tìm thấy kì thực tập");

		if (request.StudentList == null)
			throw new Exception("Danh sách học sinh rỗng");

		var internshipTerm = await _context.InternshipTerms.Where(i => i.Id == request.IntershipTermId && i.DeleteFlag == false).AsNoTracking().FirstOrDefaultAsync();

		if (internshipTerm == null)
			throw new Exception($"Không tìm thấy kì thực tập với Id: {request.IntershipTermId}");

		List<ImportStudentModel> newStudentList = new List<ImportStudentModel>();
		List<Guid> existedStudentIdList = new List<Guid>();

		using (var stream = new MemoryStream())
		{
			await request.StudentList.CopyToAsync(stream);
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			using (var package = new ExcelPackage(stream))
			{
				var worksheet = package.Workbook.Worksheets[0];
				var rowcount = worksheet.Dimension.Rows;
				var columnCount = worksheet.Dimension.Columns;
				for (int row = 2; row <= rowcount; row++)
				{
					var importStudentModel = new ImportStudentModel();
					int flag = 0;
					for (int column = 1; column <= 2; column++)
					{
						if (worksheet.Cells[row, column].Value.ToString() != null)
						{
							var col = worksheet.Cells[1, column].Value.ToString();
							switch (col)
							{
								case "MSSV":
									string code = worksheet.Cells[row, column].Value.ToString() ?? "";
									Guid studentId = await CheckCode(code);
									if (studentId != Guid.Empty)
									{
										existedStudentIdList.Add(studentId);
										flag = 1;
										break;
									}
									importStudentModel.StudentCode = code;
									break;
								case "Họ và Tên":
									string name = worksheet.Cells[row, column].Value.ToString() ?? "";
									importStudentModel.StudentFullName = name;
									break;
								default:
									break;
							}
						}
					}
					if (flag == 0)
					{
						newStudentList.Add(importStudentModel);
					}
				}
			}
		}

		if (existedStudentIdList.Count < 0 && newStudentList.Count < 0)
			throw new Exception("File excel không hợp lệ");

		if (existedStudentIdList.Count > 0)
		{
			await AddExistedStudent(existedStudentIdList, request.CurrentUserId ?? Guid.NewGuid(), request.IntershipTermId);
		}

		if (newStudentList.Count > 0)
		{
			await AddNewStudent(newStudentList, request.CurrentUserId ?? Guid.NewGuid(), request.IntershipTermId);
		}

		return true;
	}

	private async Task<bool> AddNewStudent(List<ImportStudentModel> importStudentModelList, Guid currentUserId, Guid internshipTermId)
	{
		List<Guid> studentIdList = new List<Guid>();

		foreach (var student in importStudentModelList)
		{
			User user = new User()
			{
				Id = Guid.NewGuid(),
				Email = student.StudentCode + "@gmail.com",
				Password = "123456",
				Phone = "",
				Address = "",
				RoleId = RoleConstant.Student,
				DeleteFlag = false,
				CreatedDate = DateTime.Now,
				LastModifiedDate = DateTime.Now,
				CreatedApplicationUserId = currentUserId,
				LastModifiedApplicationUserId = currentUserId
			};
			_context.Users.Add(user);

			Student newStudent = new Student()
			{
				Id = user.Id,
				FullName = student.StudentFullName,
				Code = student.StudentCode,
				StartDate = DateTime.Now,
				Major = "",
				DeleteFlag = false,
				CreatedDate = DateTime.Now,
				LastModifiedDate = DateTime.Now,
				CreatedApplicationUserId = currentUserId,
				LastModifiedApplicationUserId = currentUserId
			};
			_context.Students.Add(newStudent);

			studentIdList.Add(user.Id);
		}
		await _context.SaveChangesAsync();

		if (studentIdList.Count > 0)
		{
			return await AddExistedStudent(studentIdList, currentUserId, internshipTermId);
		}

		return false;
	}

	private async Task<bool> AddExistedStudent(List<Guid> studentIdList, Guid currentUserId, Guid internshipTermId)
	{
		List<InternshipRegistration> internshipRegistrationList = new List<InternshipRegistration>();
		var registrationStatus = await _context.RegistrationStatus.FindAsync(RegistrationStatusConstant.Pending);
		foreach (var studentId in studentIdList)
		{
			var checkInternshipRegistration = await _context.InternshipRegistrations.Where(i => i.StudentId == studentId
																								&& i.InternshipTermId == internshipTermId
																								&& i.DeleteFlag == false).AsNoTracking().FirstOrDefaultAsync();
			if (checkInternshipRegistration != null)
				continue;

			InternshipRegistration internshipRegistration = new InternshipRegistration();
			internshipRegistration.Id = Guid.NewGuid();
			internshipRegistration.InternshipTermId = internshipTermId;
			internshipRegistration.StudentId = studentId;
			internshipRegistration.StatusId = registrationStatus.Id;
			internshipRegistration.DeleteFlag = false;
			internshipRegistration.CreatedDate = DateTime.Now;
			internshipRegistration.LastModifiedDate = DateTime.Now;
			internshipRegistration.CreatedApplicationUserId = currentUserId;
			internshipRegistration.LastModifiedApplicationUserId = currentUserId;

			internshipRegistrationList.Add(internshipRegistration);
		}

		_context.InternshipRegistrations.AddRange(internshipRegistrationList);
		int rows = await _context.SaveChangesAsync();

		return rows > 0;
	}

	private async Task<Guid> CheckCode(string code)
	{
		var studentId = await _context.Students.Where(s => s.Code == code).Select(s => s.Id).FirstOrDefaultAsync();

		return studentId;
	}

	public async Task<StudentDto> Update(AddOrUpdateStudent request)
	{
		if(request.Id == null || request.Id == Guid.Empty)
		{
			throw new ApplicationException("Không tìm thấy tài khoản");
		}
		var student = await _context.Students.Include(s => s.User).Include(s => s.StudentSkills)
						   .Where(s => s.Id == request.Id)
						   .FirstOrDefaultAsync();
		if (student == null)
		{
			throw new ApplicationException("Không tìm thấy tài khoản");
		}

		var exist = await _context.Users
						  .FirstOrDefaultAsync(s => s.Email == student.User.Email && s.Id != student.Id);
		if (exist != null)
		{
			throw new ApplicationException("Email đã được sử dụng");
		}

		student.FullName = request.FullName ?? student.FullName;
		student.Introduction = request.Introduction ?? student.Introduction;
		student.User.Email = request.Email ?? student.User.Email;
		student.User.Phone = request.Phone ?? student.User.Phone;
		student.User.Address = request.Address ?? student.User.Address;

		_context.Students.Update(student);

		if (student.StudentSkills != null && student.StudentSkills.Any())
		{
			_context.StudentSkills.RemoveRange(student.StudentSkills);
		}

		foreach(var skill in request.Skills!)
		{
			var tmp = new StudentSkill()
			{
				StudentId = student.Id,
				SkillId = skill,
			};
			_context.StudentSkills.Add(tmp);
		}

		await _context.SaveChangesAsync();

		return await GetById(student.Id) ?? new StudentDto();
	}
}
