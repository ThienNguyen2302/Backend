using AutoMapper;
using AutoMapper.QueryableExtensions;
using TDTU.API.Dtos.InternshipOrderDTO;
using TDTU.API.Models.InternshipOrderModel;

namespace TDTU.API.Implements;

public class InternshipOrderService : IInternshipOrderService
{
	private readonly IDataContext _context;
	private readonly IMapper _mapper;
	public InternshipOrderService(IDataContext context, IMapper mapper)
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

	private async Task<InternshipRegistration> FindRegistration(Guid id, Guid studentId)
	{
		var registration = await _context.InternshipRegistrations
										 .FirstOrDefaultAsync(s => s.Id == id && s.StudentId == studentId);

		if (registration == null)
		{
			throw new ApplicationException($"Không tìm dữ liệu thực tập với Id: {id}");
		}

		return registration;
	}

	public async Task<InternshipOrderDto> Add(InternshipOrderAddOrUpdate request)
	{
		var student = await FindStudent(request.StudentId);
		var registration = await FindRegistration(request.RegistrationId, request.StudentId);

		var exist = await _context.InternshipOrders
								  .Where(s => s.RegistrationId == registration.Id &&
											  (s.StatusId == OrderStatusConstant.Pending ||
												s.StatusId == OrderStatusConstant.Accepted))
								  .FirstOrDefaultAsync();

		if (exist != null) throw new ApplicationException($"Đang có đơn thực tập được duyệt không thể tạo thêm");

		var status = await _context.OrderStatus.FindAsync(OrderStatusConstant.Pending);
		if (status == null) throw new ApplicationException($"Trạng thái không hợp lệ");

		var order = new InternshipOrder()
		{
			Status = status,
			StatusId = status.Id,
			RegistrationId = registration.Id,
			Registration = registration,
			Position = request.Position,
			Company = request.Company,
			StartDate = request.StartDate,
			EndDate = request.EndDate,
			TaxCode = request.TaxCode,
			IsAttach = false,
			CreatedApplicationUserId = request.CreatedApplicationUserId,
			LastModifiedApplicationUserId = request.LastModifiedApplicationUserId,
		};

		_context.InternshipOrders.Add(order);
		await _context.SaveChangesAsync();

		return _mapper.Map<InternshipOrderDto>(order);
	}

	public async Task<bool> DeleteByIds(DeleteRequest request)
	{
		if (request.Ids == null) throw new ApplicationException("Không tìm thấy tham số Id.");
		List<Guid> ids = request.Ids.Select(m => Guid.Parse(m)).ToList();
		var query = await _context.InternshipOrders.Where(m => ids.Contains(m.Id)).ToListAsync();
		if (query == null || query.Count == 0) throw new ApplicationException($"Không tìm thấy trong dữ liệu có Id: {string.Join(";", request.Ids)}");

		foreach (var item in query)
		{
			item.DeleteFlag = true;
			item.LastModifiedDate = DateTime.Now;
			item.LastModifiedApplicationUserId = request.ApplicationUserId;
		}
		_context.InternshipOrders.UpdateRange(query);

		int rows = await _context.SaveChangesAsync();
		return rows > 0;
	}

	public async Task<InternshipOrderDto> GetById(Guid id)
	{
		var data = await _context.InternshipOrders
								 .Include(s => s.Status)
								 .Include(s => s.Registration).ThenInclude(s => s.Student)
								 .Where(s => s.Id == id && s.Registration != null && s.Registration.Student != null)
								 .ProjectTo<InternshipOrderDto>(_mapper.ConfigurationProvider)
								 .FirstOrDefaultAsync();

		return data;

	}

	public async Task<PaginatedList<InternshipOrderDto>> GetPagination(PaginationRequest request)
	{
		var query = _context.InternshipOrders
							.OrderByDescending(s => s.CreatedDate)
							.Include(s => s.Status)
							.Include(s => s.Registration).ThenInclude(s => s.Student)
							.Where(s => s.Registration != null && s.Registration.Student != null && s.IsAttach == false)
							.ProjectTo<InternshipOrderDto>(_mapper.ConfigurationProvider)
							.AsNoTracking().AsEnumerable();

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.Student!.FullName.ToLower().Contains(text) ||
									 x.Student!.Code.ToLower().Contains(text) ||
									 x.Company.ToLower().Contains(text));
		}

		if (request.RegistrationId != null && request.RegistrationId != Guid.Empty)
		{
			query = query.Where(x => x.RegistrationId == request.RegistrationId);
		}

		if (request.TermId != null && request.TermId != Guid.Empty)
		{
			query = query.Where(x => x.TermId == request.TermId);
		}

		var result = query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToList();
		return new PaginatedList<InternshipOrderDto>(result, result.Count(), request.PageIndex, request.PageSize);
	}

	public async Task<InternshipOrderDto> Update(InternshipOrderAddOrUpdate request)
	{
		var order = await _context.InternshipOrders
								  .Include(s => s.Registration)
								  .FirstOrDefaultAsync(s => s.Id == request.Id);

		if (order == null || order.Registration == null)
		{
			throw new ApplicationException($"Không tìm thấy đơn yêu cầu với Id: {request.StudentId}");
		}

		if (order.Registration.IsExpired == true)
		{
			throw new ApplicationException("Kì thực tập đã hết hạn không thể chỉnh sửa");
		}

		order.Company = request.Company;
		order.Position = request.Position;
		order.StartDate = request.StartDate;
		order.EndDate = request.EndDate;
		order.TaxCode = request.TaxCode;
		order.LastModifiedApplicationUserId = request.LastModifiedApplicationUserId;
		order.LastModifiedDate = DateTime.Now;

		_context.InternshipOrders.Update(order);
		await _context.SaveChangesAsync();

		return await GetById(order.Id);
	}

	public async Task<InternshipOrderDto> UpdateStatus(InternshipOderUpdateStatus request)
	{
		var internshipOrder = await _context.InternshipOrders
											.Include(s => s.Registration)
										    .Where(i => i.Id == request.InternshipOderId)
											.FirstOrDefaultAsync();

		if (internshipOrder == null || internshipOrder.Registration == null)
		{
			throw new Exception($"Không tìm thấy đơn yêu cầu với Id: {request.InternshipOderId}");
		}

		if (internshipOrder.StatusId == OrderStatusConstant.Declined || internshipOrder.StatusId == OrderStatusConstant.Accepted)
		{
			throw new Exception($"Đơn yêu cầu đã được duyệt không thể cập nhật");
		}

		if (internshipOrder.Registration.IsExpired == true)
		{
			throw new ApplicationException("Kì thực tập đã hết hạn không thể chỉnh sửa");
		}

		var internshipOrderStatus = await _context.OrderStatus
												  .Where(o => o.Id == request.Status)
												  .FirstOrDefaultAsync();

		if (internshipOrderStatus == null)
		{
			throw new Exception($"Trạng thái không hợp lệ: {request.Status}");
		}
            
		internshipOrder.StatusId = internshipOrderStatus.Id;
		internshipOrder.LastModifiedDate = DateTime.Now;
		internshipOrder.LastModifiedApplicationUserId = request.LastModifiedApplicationUserId;

		if (request.Status == OrderStatusConstant.Accepted)
		{
			internshipOrder.Registration!.StatusId = RegistrationStatusConstant.Inprogress;
			_context.InternshipRegistrations.Update(internshipOrder.Registration);
		}

		_context.InternshipOrders.Update(internshipOrder);
		await _context.SaveChangesAsync();

		return _mapper.Map<InternshipOrderDto>(internshipOrder);
    }
}
