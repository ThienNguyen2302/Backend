using Microsoft.AspNetCore.Mvc;
using TDTU.API.Dtos.RegularJobDTO;
using TDTU.API.Models.InternshipJobModel;
using TDTU.API.Models.RegularJobModel;

namespace TDTU.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RegularJobController : BaseController
	{
		private readonly IRegularJobService _regularJobService;
		public RegularJobController(IRegularJobService regularJobService)
		{
			_regularJobService = regularJobService;
		}

		[HttpGet]
		public async Task<IActionResult> All([FromQuery] BaseRequest request)
		{
			var data = await _regularJobService.GetAll(request);
			var response = Result<List<RegularJobDto>>.Success(data);
			return Ok(response);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute] Guid id)
		{
			var user = GetUserId();
			var data = await _regularJobService.GetById(id, user);
			var response = Result<RegularJobDto>.Success(data);
			return Ok(response);
		}

		[HttpGet("filter")]
		public async Task<IActionResult> Filter([FromQuery] FilterRequest request)
		{
			var data = await _regularJobService.GetFilter(request);
			var response = Result<List<RegularJobDto>>.Success(data);
			return Ok(response);
		}

		[HttpGet("pagination")]
		public async Task<IActionResult> Pagination([FromQuery] JobPaginationRequest request)
		{
			var data = await _regularJobService.GetPagination(request);
			var response = Result<PaginatedList<RegularJobDto>>.Success(data);
			return Ok(response);
		}

		[HttpGet("suggest")]
		public async Task<IActionResult> Suggest([FromQuery] SuggestRequest request)
		{
			var data = await _regularJobService.Suggest(request);
			var response = Result<PaginatedList<RegularJobDto>>.Success(data);
			return Ok(response);
		}

		[HttpDelete]
		public async Task<IActionResult> Delete([FromBody] DeleteRequest request)
		{
			var data = await _regularJobService.DeleteByIds(request);
			var response = Result<bool>.Success(data);
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] RegularJobAddOrUpdate request)
		{
			request.CompanyId = GetUserId() ?? Guid.Empty;
			var data = await _regularJobService.Add(request);
			var response = Result<RegularJobDto>.Success(data);
			return Ok(response);
		}

		[HttpPut]
		public async Task<IActionResult> Update([FromBody] RegularJobAddOrUpdate request)
		{
			request.CompanyId = GetUserId() ?? Guid.Empty;
			var data = await _regularJobService.Update(request);
			var response = Result<RegularJobDto>.Success(data);
			return Ok(response);
		}
	}
}
