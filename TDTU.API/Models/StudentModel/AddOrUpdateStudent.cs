namespace TDTU.API.Models.StudentModel;

public class AddOrUpdateStudent
{
	public Guid? Id { get; set; }	
	public string? FullName { get; set; } = string.Empty;
	public string? Email { get; set; } = string.Empty;
	public string? Phone { get; set; } = string.Empty;
	public string? Address { get; set; } = string.Empty;
	public string? Introduction { get; set; } = string.Empty;
	public List<Guid>? Skills { get; set; } = new List<Guid>();
}
