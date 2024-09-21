namespace TDTU.API.Models.InternshipRegistrationModel;

public class InternshipRegistrationAddOrUpdate : AddOrUpdateRequest
{
	public Guid StudentId { get; set; }
	public Guid TermId { get; set; }
	public string Code { get; set; }
	public string FullName { get; set; }
}
