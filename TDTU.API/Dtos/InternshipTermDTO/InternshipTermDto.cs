using AutoMapper;

namespace TDTU.API.Dtos.InternshipTermDTO;

public class InternshipTermDto : BaseEntityDto
{
	public string Name { get; set; } = string.Empty;
	public DateTime StartDate { get; set; } = DateTime.Now;
	public DateTime EndDate { get; set; } = DateTime.Now.AddMonths(3);
	public bool IsExpired { get; set; } = false;

	private class Mapping : Profile
	{
		public Mapping()
		{
			CreateMap<InternshipTerm, InternshipTermDto>();
		}
	}
}
