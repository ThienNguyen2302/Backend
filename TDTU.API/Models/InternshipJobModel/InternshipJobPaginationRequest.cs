namespace TDTU.API.Models.InternshipJobModel;

public class JobPaginationRequest : PaginationRequest
{
	public Guid? SkillId { get; set; }
	public string? RoleId { get; set; }
}
