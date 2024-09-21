using AutoMapper;
using TDTU.API.Dtos.ApplicationStatusDTO;
namespace TDTU.API.Dtos.InternshipJobApplicationDTO;

public class InternshipJobApplicationDto : BaseEntityDto
{
	public Guid JobId { get; set; }
	public Guid StudentId { get; set; }
	public string StatusId { get; set; }
	public ApplicationStatusDto Status { get; set; }
	public string Code { get; set; } = string.Empty;
	public string FullName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public string CV { get; set; } = string.Empty;
	public string Introduce { get; set; } = string.Empty;
	public string? Company { get; set; } = string.Empty;
	public string? Position { get; set; } = string.Empty;
	public DateTime? CreatedDate { get; set; }
	private class Mapping : Profile
	{
		public Mapping()
		{
			CreateMap<InternshipJobApplication, InternshipJobApplicationDto>()
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
		}
	}
}
