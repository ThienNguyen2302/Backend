using Microsoft.AspNetCore.Mvc;
using TDTU.API.Dtos.UserDTO;
using TDTU.API.Filters;
using TDTU.API.Models.UserModel;

namespace TDTU.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthenticationController : BaseController
	{
		private readonly IUserService _userService;
		public AuthenticationController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginModel request)
		{
			var data = await _userService.Login(request);
			var response = Result<LoginDto>.Success(data);
			return Ok(response);
		}

		[HttpPost("registration")]
		[Role("ADMIN","STUDENT")]
		public async Task<IActionResult> Registration()
		{
			return Ok("registered !!!");
		}

		[HttpPut("change-password")]
		public async Task<IActionResult> ChangePassword()
		{
			return Ok("changed !!!");
		}

		[HttpPut("forgot-password")]
		public async Task<IActionResult> ForgotPassword()
		{
			return Ok("updated !!!");
		}
	}
}
