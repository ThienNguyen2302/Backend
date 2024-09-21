using Microsoft.AspNetCore.Mvc;
using TDTU.API.Dtos.ApplicationStatusDTO;

namespace TDTU.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ApplicationStatusController : BaseController
	{
		private readonly IApplicationStatusService _service;
		public ApplicationStatusController(IApplicationStatusService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<IActionResult> All([FromQuery] BaseRequest request)
		{
			var data = await _service.GetAll(request);
			var response = Result<List<ApplicationStatusDto>>.Success(data);
			return Ok(response);
		}

		[HttpGet("filter")]
		public async Task<IActionResult> Filter([FromQuery] FilterRequest request)
		{
			var data = await _service.GetFilter(request);
			var response = Result<List<ApplicationStatusDto>>.Success(data);
			return Ok(response);
		}

		[HttpGet("pagination")]
		public async Task<IActionResult> Pagination([FromQuery] PaginationRequest request)
		{
			var data = await _service.GetPagination(request);
			var response = Result<PaginatedList<ApplicationStatusDto>>.Success(data);
			return Ok(response);
		}
	}
}
