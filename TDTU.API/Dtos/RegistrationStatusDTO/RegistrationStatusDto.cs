using AutoMapper;

namespace TDTU.API.Dtos.RegistrationStatusDTO;

public class RegistrationStatusDto : BaseStatusDto
{
	public string Name { get; set; }
	public string Description { get; set; }

	private class Mapping : Profile
	{
		public Mapping()
		{
			CreateMap<RegistrationStatus, RegistrationStatusDto>();
		}
	}
}
