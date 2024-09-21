using AutoMapper;

namespace TDTU.API.Dtos.SkillDTO;

public class SkillDto : BaseEntityDto
{
	public string? Name { get; set; } = string.Empty;
	public string? Description { get; set; } = string.Empty;
	public string? Sort { get; set; } = string.Empty;
	private class Mapping : Profile
	{
		public Mapping()
		{
			CreateMap<Skill, SkillDto>();
		}
	}
}
