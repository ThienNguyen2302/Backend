using Microsoft.AspNetCore.Mvc;
using TDTU.API.Dtos.UserDTO;

namespace TDTU.API.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class UserController : BaseController
	{
		private readonly IUserService _userService;
		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet]
		public async Task<IActionResult> All([FromQuery] BaseRequest request)
		{
			var data = await _userService.GetAll(request);
			var response = Result<List<UserDto>>.Success(data);
			return Ok(response);
		}

		[HttpGet("profile")]
		public async Task<IActionResult> Profile()
		{
			var id = GetUserId();
			var data = await _userService.Profile(id ?? Guid.Empty);
			var response = Result<ProfileDto>.Success(data);
			return Ok(response);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute] Guid id)
		{
			var data = await _userService.GetById(id);
			var response = Result<UserDto>.Success(data);
			return Ok(response);
		}

		[HttpGet("filter")]
		public async Task<IActionResult> Filter([FromQuery] FilterRequest request)
		{
			var data = await _userService.GetFilter(request);
			var response = Result<List<UserDto>>.Success(data);
			return Ok(response);
		}

		[HttpGet("pagination")]
		public async Task<IActionResult> Pagination([FromQuery] PaginationRequest request)
		{
			var data = await _userService.GetPagination(request);
			var response = Result<PaginatedList<UserDto>>.Success(data);
			return Ok(response);
		}

		[HttpDelete]
		public async Task<IActionResult> Delete([FromBody] DeleteRequest request)
		{
			var data = await _userService.DeleteByIds(request);
			var response = Result<bool>.Success(data);
			return Ok(response);
		}
	}
}
