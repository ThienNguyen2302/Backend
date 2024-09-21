using AutoMapper;

namespace TDTU.API.Dtos.UserDTO;

public class ProfileDto : UserDto
{
	public string DisplayName { get; set; } = string.Empty;
	public string Code { get; set; } = string.Empty;
	public Guid? TermId { get; set; }
	public Guid? RegistrationId { get; set; }
	public string? Introduction { get; set; }
    public string? Skill { get; set; }

    private class Mapping : Profile
	{
		public Mapping()
		{
			CreateMap<User, ProfileDto>()
				.ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src =>
					src.RoleId == RoleConstant.Admin ? src.Email :
					src.RoleId == RoleConstant.Student ? ( src.Student != null ? src.Student.FullName : "" ) :
					src.RoleId == RoleConstant.Company ? (src.Company != null ? src.Company.Name : "") :
					string.Empty
				));
		}
	}
}
