using AutoMapper;
using TDTU.API.Dtos.SkillDTO;
using TDTU.API.Dtos.UserDTO;

namespace TDTU.API.Dtos.StudentDTO;

public class StudentDto : UserDto
{
	public string FullName { get; set; } = string.Empty;
	public string Code { get; set; } = string.Empty;
	public DateTime? StartDate { get; set; } = DateTime.Now.AddYears(-4);
	public string? Major { get; set; } = string.Empty;
	public string? Introduction { get; set; } = string.Empty;
	public List<SkillDto> Skills { get; set; }
	private class Mapping : Profile
	{
		public Mapping()
		{
			CreateMap<Student, StudentDto>()
				.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email ?? ""))
				.ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User.Phone ?? ""))
				.ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.User.Address ?? ""))
				.ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.User.RoleId ?? ""));
		}
	}
}
