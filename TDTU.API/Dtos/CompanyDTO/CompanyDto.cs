using AutoMapper;
using TDTU.API.Dtos.UserDTO;

namespace TDTU.API.Dtos.CompanyDTO;

public class CompanyDto : UserDto
{
	public string? Name { get; set; } = string.Empty;
	public string? TaxCode { get; set; } = string.Empty;
	public string? Logo { get; set; } = string.Empty;
	public string? Description { get; set; } = string.Empty;
	private class Mapping : Profile
	{
		public Mapping()
		{
			CreateMap<Company, CompanyDto>()
				.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email ?? ""))
				.ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User.Phone ?? ""))
				.ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.User.Address ?? ""))
				.ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.User.RoleId ?? ""));
		}
	}
}
