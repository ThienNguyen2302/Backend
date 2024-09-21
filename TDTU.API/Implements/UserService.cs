using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TDTU.API.Dtos.UserDTO;
using TDTU.API.Models.UserModel;

namespace TDTU.API.Implements;

public class UserService : IUserService
{
	private readonly IDataContext _context;
	private readonly IMapper _mapper;
	public UserService(IDataContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<bool> DeleteByIds(DeleteRequest request)
	{
		if (request.Ids == null) throw new ApplicationException("Không tìm thấy tham số Id.");
		List<Guid> ids = request.Ids.Select(m => Guid.Parse(m)).ToList();
		var query = await _context.Users.Where(m => ids.Contains(m.Id)).ToListAsync();
		if (query == null || query.Count == 0) throw new ApplicationException($"Không tìm thấy trong dữ liệu có Id: {string.Join(";", request.Ids)}");

		foreach (var item in query)
		{
			item.DeleteFlag = true;
			item.LastModifiedDate = DateTime.Now;
			item.LastModifiedApplicationUserId = request.ApplicationUserId;
		}
		_context.Users.UpdateRange(query);

		int rows = await _context.SaveChangesAsync();
		return rows > 0;
	}

	public async Task<List<UserDto>> GetAll(BaseRequest request)
	{
		List<UserDto> users = await _context.Users.Include(s => s.Role)
											.ProjectTo<UserDto>(_mapper.ConfigurationProvider)
											.ToListAsync();
		return users;
	}

	public async Task<UserDto> GetById(Guid id)
	{
		UserDto? user = await _context.Users.Include(s => s.Role)
							  .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
							  .FirstOrDefaultAsync();
		return user;
	}

	public async Task<List<UserDto>> GetFilter(FilterRequest request)
	{
		var query = _context.Users
					.OrderByDescending(s => s.CreatedDate)
					.ProjectTo<UserDto>(_mapper.ConfigurationProvider)
					.AsNoTracking();

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.Phone.ToLower().Contains(text) ||
									 x.Email.ToLower().Contains(text));
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

	public async Task<PaginatedList<UserDto>> GetPagination(PaginationRequest request)
	{
		var query = _context.Users
					.OrderByDescending(s => s.CreatedDate)
					.ProjectTo<UserDto>(_mapper.ConfigurationProvider)
					.AsNoTracking();

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.Email.ToLower().Contains(text) ||
									 x.Phone.ToLower().Contains(text));
		}

		PaginatedList<UserDto> paging = await query.PaginatedListAsync(request.PageIndex, request.PageSize);
		return paging;
	}

	public async Task<LoginDto> Login(LoginModel request)
	{
		var user = await _context.Users.Where(s => s.Email == request.Email && s.Password == request.Password)
								 .Include(s => s.Student).Include(s => s.Company)
								 .ProjectTo<ProfileDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

		if (user == null) throw new ApplicationException("Tên đang nhập hoặc mật khẩu không chính xác");

		if (user.RoleId == null) throw new ApplicationException("Tài khoản không đủ quyền truy cập");

		if (user.RoleId == RoleConstant.Student) await GetStudentTerm(user);
		else await GetAdminTerm(user);

		LoginDto result = new LoginDto()
		{
			User = user,
			Token = GenerateToken(user.Id, user.Email, user.RoleId),
			ValidTo = JWTConstant.ValidTo()
		};
		return result;
	}

	public async Task<ProfileDto> Profile(Guid id)
	{
		var user = await _context.Users.Include(s => s.Student)
						 .ProjectTo<ProfileDto>(_mapper.ConfigurationProvider)
						 .FirstOrDefaultAsync(s => s.Id == id);

		if (user == null) throw new ApplicationException("Không tìm thấy dữ liệu người dùng");

		if (user.RoleId == null) throw new ApplicationException("Tài khoản không đủ quyền truy cập");

		if (user.RoleId == RoleConstant.Student) await GetStudentTerm(user);
		else await GetAdminTerm(user);

		var student = await _context.Students.Where(sp => sp.Id == id).FirstOrDefaultAsync();
		if(student != null)
		{
			user.Introduction = student.Introduction;
			user.Skill = student.Skill;
			user.Code = student.Code;
		}

		return user;
	}

	private async Task GetStudentTerm(ProfileDto profile)
	{
		Guid id = profile.Id;
		var term = await GetCurrentTerm();
		if(term != null)
		{
			profile.TermId = term.Id;
			var registration = await _context.InternshipRegistrations
									 .Where(s => s.StudentId == id && s.InternshipTermId == term.Id)
									 .FirstOrDefaultAsync();
			
			if(registration != null) profile.RegistrationId = registration.Id;
		}
	}

	private async Task GetAdminTerm(ProfileDto profile)
	{
		Guid id = profile.Id;
		var term = await GetCurrentTerm();
		if (term != null) profile.TermId = term.Id;
	}

	private async Task<InternshipTerm?> GetCurrentTerm()
	{
		DateTime now = DateTime.Now;
		var term = await _context.InternshipTerms
						 .Where(s => s.IsExpired != true && s.StartDate < now && now < s.EndDate)
						 .FirstOrDefaultAsync();
		return term;
	}

	private string GenerateToken(Guid id, string email, string role)
	{
		var authClaims = new[]
		{
			new Claim(JWTClaimsTypeConstant.Id, id.ToString()),
			new Claim(JWTClaimsTypeConstant.Email, email),
			new Claim(JWTClaimsTypeConstant.Role, role)
		};

		SymmetricSecurityKey authSigningKey = new(Encoding.UTF8.GetBytes(JWTConstant.Secret));

		JwtSecurityToken token = new(
			JWTConstant.ValidIssuer,
			JWTConstant.ValidAudience,
			expires: JWTConstant.ValidTo(),
			claims: authClaims,
			signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature)
		);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}
