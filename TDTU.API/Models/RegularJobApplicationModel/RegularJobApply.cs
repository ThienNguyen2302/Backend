namespace TDTU.API.Models.RegularJobApplicationModel;

public class RegularJobApplyRequest
{
	public Guid StudentId { get; set; }
	public Guid JobId { get; set; }
	public string FullName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public string CV { get; set; } = string.Empty;
	public string? Introduce { get; set; } = string.Empty;
}
