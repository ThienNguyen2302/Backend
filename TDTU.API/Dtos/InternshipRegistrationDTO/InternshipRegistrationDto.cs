using TDTU.API.Dtos.RegistrationStatusDTO;

namespace TDTU.API.Dtos.InternshipRegistrationDTO;

public class InternshipRegistrationDto : BaseEntityDto
{
	public Guid? StudentId { get; set; }
	public Guid? InternshipTermId { get; set; }
	public string Code { get; set; }
	public string FullName { get; set; }
	public string? StatusId { get; set; }
	public RegistrationJobDto Job { get; set; }
	public RegistrationStatusDto Status { get; set; }
}

public class RegistrationJobDto : BaseEntityDto
{
	public string Position { get; set; } = string.Empty;
	public string Company { get; set; } = string.Empty;
	public string StatusId { get; set; } = string.Empty;
}