namespace TDTU.API.Models.InternshipRegistrationModel;

public class AttachStudentToJob
{
	public Guid JobId { get; set; }
	public Guid ModifiedUser { get; set; }
	public List<Guid> RegistrationIds { get; set; }
}
