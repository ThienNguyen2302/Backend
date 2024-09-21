using AutoMapper;
using TDTU.API.Dtos.CompanyDTO;
using TDTU.API.Dtos.SkillDTO;

namespace TDTU.API.Dtos.RegularJobDTO;

public class RegularJobDto : BaseEntityDto
{
	public string Name { get; set; } = string.Empty;
	public decimal SalaryMin { get; set; }
	public decimal SalaryMax { get; set; }
	public string Description { get; set; } = string.Empty;
	public DateTime ExpireDate { get; set; }
	public Guid? CompanyId { get; set; }
	public CompanyDto Company { get; set; }
	public List<SkillDto> Skills { get; set;} = new List<SkillDto>();
	public string? ApplyStatus { get; set; } = string.Empty;
	private class Mapping : Profile
	{
		public Mapping()
		{
			CreateMap<RegularJob, RegularJobDto>()
				.ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company))
				.ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.Skills));
		}
	}
}
