using Microsoft.AspNetCore.Mvc;
using TDTU.API.Dtos.CompanyDTO;
using TDTU.API.Models.CompanyModel;

namespace TDTU.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CompanyController : BaseController
	{
		private readonly ICompanyService _companyService;
		public CompanyController(ICompanyService companyService)
		{
			_companyService = companyService;
		}

		[HttpGet]
		public async Task<IActionResult> All([FromQuery] BaseRequest request)
		{
			var data = await _companyService.GetAll(request);
			var response = Result<List<CompanyDto>>.Success(data);
			return Ok(response);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute] Guid id)
		{
			var data = await _companyService.GetById(id);
			var response = Result<CompanyDto>.Success(data);
			return Ok(response);
		}

		[HttpGet("filter")]
		public async Task<IActionResult> Filter([FromQuery] FilterRequest request)
		{
			var data = await _companyService.GetFilter(request);
			var response = Result<List<CompanyDto>>.Success(data);
			return Ok(response);
		}

		[HttpGet("pagination")]
		public async Task<IActionResult> Pagination([FromQuery] PaginationRequest request)
		{
			var data = await _companyService.GetPagination(request);
			var response = Result<PaginatedList<CompanyDto>>.Success(data);
			return Ok(response);
		}

		[HttpDelete]
		public async Task<IActionResult> Delete([FromBody] DeleteRequest request)
		{
			var data = await _companyService.DeleteByIds(request);
			var response = Result<bool>.Success(data);
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CompanyAddOrUpdate request)
		{
			var data = await _companyService.Add(request);
			var response = Result<CompanyDto>.Success(data);
			return Ok(response);
		}

		[HttpPut]
		public async Task<IActionResult> Update([FromBody] CompanyAddOrUpdate request)
		{
			var data = await _companyService.Update(request);
			var response = Result<CompanyDto>.Success(data);
			return Ok(response);
		}
	}
}
