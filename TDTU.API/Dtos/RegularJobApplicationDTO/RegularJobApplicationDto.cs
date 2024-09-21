using AutoMapper;

namespace TDTU.API.Dtos.RegularJobApplicationDTO;

public class RegularJobApplicationDto : BaseEntityDto
{
	public Guid JobId { get; set; }
	public Guid StudentId { get; set; }
	public string FullName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public string CV { get; set; } = string.Empty;
	public string? Introduce { get; set; } = string.Empty;
	public DateTime? CreatedDate { get; set; }
	public string Company { get; set; } = string.Empty;
	public string Position { get; set; } = string.Empty;
	public decimal? SalaryMin { get; set; }
	public decimal? SalaryMax { get; set; }
	private class Mapping : Profile
	{
		public Mapping()
		{
			CreateMap<RegularJobApplication, RegularJobApplicationDto>();
		}
	}
}
