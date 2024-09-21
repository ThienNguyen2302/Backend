using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace TDTU.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BaseController : ControllerBase
	{
		protected Guid? GetUserId()
		{
			try
			{
				var handler = new JwtSecurityTokenHandler();
				string authHeader = HttpContext.Request.Headers["Authorization"];
				if (string.IsNullOrEmpty(authHeader)) return null;
				authHeader = authHeader.Replace("Bearer ", "");
				var jsonToken = handler.ReadToken(authHeader);
				var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
				if (tokenS == null) { return null; }
				var id = tokenS.Claims.First(claim => claim.Type == JWTClaimsTypeConstant.Id).Value;
				return Guid.Parse(id);
			}
			catch
			{
				return null;
			}
		}

		protected string GetRole()
		{
			try
			{
				var handler = new JwtSecurityTokenHandler();
				string authHeader = HttpContext.Request.Headers["Authorization"];
				if (string.IsNullOrEmpty(authHeader)) return "";
				authHeader = authHeader.Replace("Bearer ", "");
				var jsonToken = handler.ReadToken(authHeader);
				var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
				if (tokenS == null) { return ""; }
				var id = tokenS.Claims.First(claim => claim.Type == JWTClaimsTypeConstant.Role).Value;
				return id;
			}
			catch
			{
				return "";
			}
		}

		protected void SetCookie(HttpResponse Response,string accessToken,string refreshToken)
		{
			Response.Cookies.Append(JWTConstant.AccessTokenCookie, accessToken, new CookieOptions
			{
				HttpOnly = false,
				Secure = true,
				SameSite = SameSiteMode.None,
				Expires = JWTConstant.ValidTo()
			});

			Response.Cookies.Append(JWTConstant.RefreshTokenCookie, refreshToken, new CookieOptions
			{
				HttpOnly = false,
				Secure = true,
				SameSite = SameSiteMode.None,
				Expires = JWTConstant.RefreshTokenValidTo()
			});
		}
	}
}
