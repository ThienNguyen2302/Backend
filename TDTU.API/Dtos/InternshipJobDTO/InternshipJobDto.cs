using AutoMapper;
using TDTU.API.Dtos.CompanyDTO;
using TDTU.API.Dtos.SkillDTO;

namespace TDTU.API.Dtos.InternshipJobDTO;

public class InternshipJobDto : BaseEntityDto
{
	public string? Name { get; set; } = string.Empty;
	public string? Description { get; set; } = string.Empty;
    public int MaxStudent { get; set; }
	public int Applications { get; set; } = 0;
    public Guid CompanyId { get; set; }
	public CompanyDto Company { get; set; }
	public Guid? InternshipTermId { get; set; }
	public DateTime? StartDate { get; set; } = DateTime.Now;
	public DateTime? EndDate { get; set; } = DateTime.Now.AddMonths(3);
	public List<SkillDto> Skills { get; set; } = new List<SkillDto>();
	public string? ApplyStatus { get; set; } = string.Empty;
	private class Mapping : Profile
	{
		public Mapping()
		{
			CreateMap<InternshipJob, InternshipJobDto>()
				.ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company))
				.ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.InternshipTerm!.StartDate))
				.ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.InternshipTerm!.EndDate))
				.ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.Skills))
				.ForMember(dest => dest.Applications, opt => opt.MapFrom(src => src.Applications != null 
						? src.Applications.Where(s => s.StatusId == ApplicationStatusConstant.Interning).Count() : 0));
		}
	}
}
