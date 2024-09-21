using Microsoft.AspNetCore.Mvc;
using TDTU.API.Dtos.InternshipJobApplicationDTO;
using TDTU.API.Models.InternshipJobApplicationModel;

namespace TDTU.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class InternshipJobApplicationController : BaseController
	{
		private readonly IInternshipJobApplicationService _service;
		public InternshipJobApplicationController(IInternshipJobApplicationService service)
		{
			_service = service;
		}

		[HttpPost("apply")]
		public async Task<IActionResult> Apply([FromBody] InternshipJobApply request)
		{
			request.StudentId = GetUserId() ?? Guid.Empty;
			var data = await _service.Apply(request);
			var response = Result<InternshipJobApplicationDto>.Success(data);
			return Ok(response);
		}

		[HttpPut("status")]
		public async Task<IActionResult> SetStatus([FromBody] InternshipJobSetStatus request)
		{
			request.CompanyId = GetUserId() ?? Guid.Empty;
			var data = await _service.SetStatus(request);
			var response = Result<InternshipJobApplicationDto>.Success(data);
			return Ok(response);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute] Guid id)
		{
			var data = await _service.GetById(id);
			var response = Result<InternshipJobApplicationDto>.Success(data);
			return Ok(response);
		}

		[HttpGet("job/pagination")]
		public async Task<IActionResult> PaginationJob([FromQuery] PaginationRequest request, [FromQuery] Guid id)
		{
			var data = await _service.JobApplications(request, id);
			var response = Result<PaginatedList<InternshipJobApplicationDto>>.Success(data);
			return Ok(response);
		}

		[HttpGet("student/pagination")]
		public async Task<IActionResult> PaginationStudent([FromQuery] ApplicationPaginationRequest request)
		{
			request.UserId = GetUserId() ?? Guid.Empty;
			var data = await _service.UserHistory(request);
			var response = Result<PaginatedList<InternshipJobApplicationDto>>.Success(data);
			return Ok(response);
		}

		[HttpDelete]
		public async Task<IActionResult> Delete([FromBody] DeleteRequest request)
		{
			var data = await _service.DeleteByIds(request);
			var response = Result<bool>.Success(data);
			return Ok(response);
		}
	}
}
