namespace TDTU.API.Models.InternshipJobApplicationModel;

public class InternshipJobApply
{
	public Guid StudentId { get; set; }
	public Guid JobId { get; set; }
	public Guid TermId { get; set; }
	public string Code { get; set; } = string.Empty;
	public string FullName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public string CV { get; set; } = string.Empty;
	public string? Introduce { get; set; } = string.Empty;
}
