using Microsoft.AspNetCore.Mvc;
using TDTU.API.Dtos.InternshipJobApplicationDTO;
using TDTU.API.Dtos.InternshipJobDTO;
using TDTU.API.Dtos.InternshipOrderDTO;
using TDTU.API.Models.InternshipJobModel;
using TDTU.API.Models.InternshipOrderModel;

namespace TDTU.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class InternshipOrderController : BaseController
	{
		private readonly IInternshipOrderService _service;
		public InternshipOrderController(IInternshipOrderService service)
		{
			_service = service;
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute] Guid id)
		{
			var data = await _service.GetById(id);
			var response = Result<InternshipOrderDto>.Success(data);
			return Ok(response);
		}

		[HttpGet("pagination")]
		public async Task<IActionResult> Pagination([FromQuery] PaginationRequest request)
		{
			var data = await _service.GetPagination(request);
			var response = Result<PaginatedList<InternshipOrderDto>>.Success(data);
			return Ok(response);
		}

		[HttpPut("status")]
		public async Task<IActionResult> UpdateStatus([FromBody] InternshipOderUpdateStatus request)
		{
			request.LastModifiedApplicationUserId = GetUserId();
            var data = await _service.UpdateStatus(request);
            var response = Result<InternshipOrderDto>.Success(data);
            return Ok(response);
        }

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] InternshipOrderAddOrUpdate request)
		{
			request.StudentId = GetUserId() ?? Guid.Empty;
			var data = await _service.Add(request);
			var response = Result<InternshipOrderDto>.Success(data);
			return Ok(response);
		}

		[HttpPut]
		public async Task<IActionResult> Update([FromBody] InternshipOrderAddOrUpdate request)
		{
			var data = await _service.Update(request);
			var response = Result<InternshipOrderDto>.Success(data);
			return Ok(response);
		}
	}
}
