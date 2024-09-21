using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;

namespace TDTU.API.Filters
{
	public class RoleAttribute : Attribute, IAuthorizationFilter
	{
		private readonly List<string> _roles = new List<string>();
		public RoleAttribute(params string[] roles)
		{
			_roles = new List<string>(roles);
		}
		public void OnAuthorization(AuthorizationFilterContext context)
		{
			try
			{
				var handler = new JwtSecurityTokenHandler();
				string? authHeader = context.HttpContext.Request.Headers["Authorization"];
				if (string.IsNullOrEmpty(authHeader))
				{
					GetResponse(context);
					return;
				};
				authHeader = authHeader.Replace("Bearer ", "");
				var jsonToken = handler.ReadToken(authHeader);
				var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
				var role = tokenS!.Claims.First(claim => claim.Type == JWTClaimsTypeConstant.Role).Value;
				if (!_roles.Contains(role))
				{
					GetResponse(context);
					return;
				}
			}
			catch (Exception ex)
			{
				GetResponse(context);
				return;
			}
		}

		private void GetResponse(AuthorizationFilterContext context)
		{
			var response = Result<string>.Failure("Bạn không có quyền thao tác");
			var json = JsonConvert.SerializeObject(response);
			context.Result = new OkObjectResult(json);
		}
	}
}
