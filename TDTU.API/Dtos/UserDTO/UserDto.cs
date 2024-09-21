using AutoMapper;

namespace TDTU.API.Dtos.UserDTO;

public class UserDto : BaseEntityDto
{
	public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
	public string RoleId { get; set; } = string.Empty;
	private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<User, UserDto>();
        }
    }
}
