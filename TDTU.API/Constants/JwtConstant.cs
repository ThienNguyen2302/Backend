namespace TDTU.API.Constants;

public static class JWTConstant
{
	public const string ValidAudience = "Udemy_Web_Application";
	public const string ValidIssuer = "Udemy_Web_Application";
	public const string Secret = "my-secrect-for-udemy_web_application";
	public const string AccessTokenCookie = "udemy-access-token-cookie";
	public const string RefreshTokenCookie = "udemy-refresh-token-cookie";
	public static DateTime ValidTo()
	{
		return DateTime.Now.AddMonths(1);
	}

	public static DateTime RefreshTokenValidTo()
	{
		return DateTime.Now.AddMonths(1);
	}
}

