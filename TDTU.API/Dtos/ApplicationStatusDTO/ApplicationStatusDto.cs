using AutoMapper;

namespace TDTU.API.Dtos.ApplicationStatusDTO;

public class ApplicationStatusDto : BaseStatusDto
{
	public string Name { get; set; }
	public string Description { get; set; }

	private class Mapping : Profile
	{
		public Mapping()
		{
			CreateMap<ApplicationStatus, ApplicationStatusDto>();
		}
	}
}
