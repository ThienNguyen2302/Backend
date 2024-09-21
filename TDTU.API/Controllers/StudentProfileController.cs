using Microsoft.AspNetCore.Mvc;
using TDTU.API.Dtos.StudentDTO;
using TDTU.API.Models.StudentProfileModel;

namespace TDTU.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StudentProfileController : BaseController
	{
		private readonly IStudentProfileService _service;
		public StudentProfileController(IStudentProfileService service)
		{
			_service = service;
		}

		[HttpGet("pagination")]
		public async Task<IActionResult> Pagination([FromQuery] PaginationRequest request)
		{
			request.UserId = GetUserId();
			var data = await _service.GetPagination(request);
			var response = Result<PaginatedList<StudentProfileDto>>.Success(data);
			return Ok(response);
		}

		[HttpGet("filter")]
		public async Task<IActionResult> Filter([FromQuery] FilterRequest request)
		{
			request.UserId = GetUserId();
			var data = await _service.GetFilter(request);
			var response = Result<List<StudentProfileDto>>.Success(data);
			return Ok(response);
		}

		[HttpDelete]
		public async Task<IActionResult> Delete([FromBody] DeleteRequest request)
		{
			var data = await _service.DeleteByIds(request);
			var response = Result<bool>.Success(data);
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> Create(IFormFile file)
		{
			var data = await _service.Add(file, GetUserId() ?? Guid.Empty);
			var response = Result<StudentProfileDto>.Success(data);
			return Ok(response);
		}
	}
}
