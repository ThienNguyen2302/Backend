namespace TDTU.API.Models.StudentProfileModel;

public class AddStudentProfileRequest
{
	public string Introduction { get; set; } = string.Empty;
	public string Skill { get; set; } = string.Empty;
	public IFormFile file { get; set; }
}
