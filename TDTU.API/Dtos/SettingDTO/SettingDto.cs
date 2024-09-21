using AutoMapper;

namespace TDTU.API.Dtos.SettingDTO;

public class SettingDto
{
    public int NumberInternshipJob { get; set; }

    private class Mapping : Profile
	{
		public Mapping()
		{
			CreateMap<Setting, SettingDto>();
		}
	}
}
