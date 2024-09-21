using Microsoft.AspNetCore.Mvc;
using TDTU.API.Dtos.InternshipRegistrationDTO;
using TDTU.API.Models.InternshipRegistrationModel;

namespace TDTU.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class InternshipRegistrationController : BaseController
	{
		private readonly IInternshipRegistrationService _service;
		public InternshipRegistrationController(IInternshipRegistrationService service)
		{
			_service = service;
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute] Guid id)
		{
			var data = await _service.GetById(id);
			var response = Result<InternshipRegistrationDto>.Success(data);
			return Ok(response);
		}

		[HttpGet("pagination")]
		public async Task<IActionResult> Pagination([FromQuery] PaginationRequest request)
		{
			var data = await _service.GetPagination(request);
			var response = Result<PaginatedList<InternshipRegistrationDto>>.Success(data);
			return Ok(response);
		}

		[HttpGet("pending")]
		public async Task<IActionResult> Get([FromQuery] PaginationRequest request)
		{
			request.Status = RegistrationStatusConstant.Pending;
			var data = await _service.GetPagination(request);
			var response = Result<PaginatedList<InternshipRegistrationDto>>.Success(data);
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
		public async Task<IActionResult> Create([FromBody] InternshipRegistrationAddOrUpdate request)
		{
			var data = await _service.Add(request);
			var response = Result<InternshipRegistrationDto>.Success(data);
			return Ok(response);
		}

		[HttpPut]
		public async Task<IActionResult> Update([FromBody] InternshipRegistrationAddOrUpdate request)
		{
			var data = await _service.Update(request);
			var response = Result<InternshipRegistrationDto>.Success(data);
			return Ok(response);
		}

	}
}
