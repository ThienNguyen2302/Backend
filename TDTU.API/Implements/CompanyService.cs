using AutoMapper;
using AutoMapper.QueryableExtensions;
using TDTU.API.Dtos.CompanyDTO;
using TDTU.API.Models.CompanyModel;

namespace TDTU.API.Implements;

public class CompanyService : ICompanyService
{
	private readonly IDataContext _context;
	private readonly IMapper _mapper;
	public CompanyService(IDataContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}
	public async Task<bool> DeleteByIds(DeleteRequest request)
	{
		if (request.Ids == null) throw new ApplicationException("Không tìm thấy tham số Id.");
		List<Guid> ids = request.Ids.Select(m => Guid.Parse(m)).ToList();
		var query = await _context.Companies.Include(s => s.User).Where(m => ids.Contains(m.Id)).ToListAsync();
		if (query == null || query.Count == 0) throw new ApplicationException($"Không tìm thấy trong dữ liệu có Id: {string.Join(";", request.Ids)}");

		foreach (var item in query)
		{
			item.User.DeleteFlag = true;
			item.DeleteFlag = true;
			item.LastModifiedDate = DateTime.Now;
			item.LastModifiedApplicationUserId = request.ApplicationUserId;
		}
		_context.Companies.UpdateRange(query);

		int rows = await _context.SaveChangesAsync();
		return rows > 0;
	}

	public async Task<List<CompanyDto>> GetAll(BaseRequest request)
	{
		List<CompanyDto> data = await _context.Companies.Include(s => s.User).OrderByDescending(s => s.CreatedDate)
											  .ProjectTo<CompanyDto>(_mapper.ConfigurationProvider)
											  .ToListAsync();
		return data;
	}

	public async Task<CompanyDto> GetById(Guid id)
	{
		CompanyDto? data = await _context.Companies.Include(s => s.User).Where(s => s.Id == id)
									  .ProjectTo<CompanyDto>(_mapper.ConfigurationProvider)
									  .FirstOrDefaultAsync();
		return data;
	}

	public async Task<List<CompanyDto>> GetFilter(FilterRequest request)
	{
		var query = _context.Companies.Include(s => s.User)
							.OrderByDescending(x => x.CreatedDate)
							.ProjectTo<CompanyDto>(_mapper.ConfigurationProvider)
							.AsNoTracking();

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.Name.ToLower().Contains(text) ||
									 x.Email.ToLower().Contains(text) ||
									 x.TaxCode.ToLower().Contains(text));
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

	public async Task<PaginatedList<CompanyDto>> GetPagination(PaginationRequest request)
	{
		var query = _context.Companies.Include(s => s.User)
								  .OrderByDescending(x => x.CreatedDate)
								  .ProjectTo<CompanyDto>(_mapper.ConfigurationProvider);

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.Email.ToLower().Contains(text) ||
									 x.Phone.ToLower().Contains(text));
		}

		PaginatedList<CompanyDto> paging = await query.PaginatedListAsync(request.PageIndex, request.PageSize);
		return paging;
	}

	public async Task<CompanyDto> Update(CompanyAddOrUpdate request)
	{
		var company = await _context.Companies.Include(s => s.User).FirstOrDefaultAsync(s => s.Id == request.Id);
		if (company == null) throw new ApplicationException($"Không tìm thấy dữ liệu với Id: {request.Id}");
		await CheckDuplicate(company.Id, request.Email, request.TaxCode);

		company.Name = request.Name;
		company.TaxCode = request.TaxCode;
		company.Logo = request.Logo;
		company.Description = request.Description ?? company.Description;
		company.LastModifiedApplicationUserId = request.LastModifiedApplicationUserId;
		company.LastModifiedDate = DateTime.Now;
		company.User.Email = request.Email;
		company.User.Address = request.Address ?? company.User.Address;
		company.User.Phone = request.Phone ?? company.User.Phone;

		_context.Companies.Update(company);
		_context.Users.Update(company.User);
		await _context.SaveChangesAsync();

		return _mapper.Map<CompanyDto>(company);
	}

	private async Task CheckDuplicate(Guid? id = null,string? email = null,string? taxCode = "")
	{
		var query = _context.Companies.Include(s => s.User).AsNoTracking();
		var user = _context.Users.AsNoTracking();

		if(id != null && id != Guid.Empty)
		{
			query = query.Where(s => s.Id != id);
			user = user.Where(s => s.Id != id);
		}

		if (!string.IsNullOrEmpty(email))
		{
			var find = await user.Where(s => s.Email == email).FirstOrDefaultAsync();
			if (find != null) throw new ApplicationException($"Email đã được sử dụng: {email}");
		}

		if (!string.IsNullOrEmpty(taxCode))
		{
			query = query.Where(s => s.TaxCode == taxCode);

			var find = await query.FirstOrDefaultAsync();
			if (find != null) throw new ApplicationException($"Mã số thuế đã được sử dụng: {taxCode}");
		}

	}

	public async Task<CompanyDto> Add(CompanyAddOrUpdate request)
	{
		await CheckDuplicate(null, request.Email, request.TaxCode);

		var user = new User()
		{
			Password = "123456",
			Email = request.Email,
			Address = request.Address ?? "",
			Phone = request.Phone ?? "",
			RoleId = RoleConstant.Company,
			CreatedApplicationUserId = request.CreatedApplicationUserId
		};
		await _context.Users.AddAsync(user);

		var company = new Company()
		{
			Id = user.Id,
			User = user,
			Name = request.Name,
			TaxCode = request.TaxCode,
			Description = request.Description ?? "",
			Logo = request.Logo,
			CreatedApplicationUserId = request.CreatedApplicationUserId
		};
		_context.Companies.Add(company);
		await _context.SaveChangesAsync();
		return _mapper.Map<CompanyDto>(company);
	}

}
