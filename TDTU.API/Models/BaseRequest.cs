namespace TDTU.API.Models;

public class BaseRequest 
{
	public Guid? UserId { get; set; }
	public Guid? TermId { get; set; }
	public Guid? RegistrationId { get; set; }
	public string? TextSearch { get; set; }
	public string? Status { get; set; }
}

public static class OrderDir
{
	public static string Desc { get; } = nameof(Desc);
	public static string Asc { get; } = nameof(Asc);
}