using AutoMapper;
using TDTU.API.Dtos.OrderStatusDTO;
using TDTU.API.Dtos.StudentDTO;
namespace TDTU.API.Dtos.InternshipOrderDTO;

public class InternshipOrderDto : BaseEntityDto
{
	public Guid? TermId { get; set; }
	public Guid? RegistrationId { get; set; }
	public string? StatusId { get; set; }
	public OrderStatusDto? Status { get; set; } = new OrderStatusDto();
	public StudentDto? Student { get; set; } = new StudentDto();
	public string Company { get; set; } = string.Empty;
	public string TaxCode { get; set; } = string.Empty;
	public string Position { get; set; } = string.Empty;
	public DateTime StartDate { get; set; } = DateTime.Now;
	public DateTime EndDate { get; set; } = DateTime.Now.AddMonths(3);

	private class Mapping : Profile
	{
		public Mapping()
		{
			CreateMap<InternshipOrder, InternshipOrderDto>()
				.ForMember(dest => dest.TermId, opt => opt.MapFrom(src => src.Status))
				.ForMember(dest => dest.Student, opt =>
					opt.MapFrom(src => src.Registration != null && src.Registration.Student != null
						? src.Registration.Student : null))
				.ForMember(dest => dest.TermId, opt =>
					opt.MapFrom(src => src.Registration != null ? src.Registration.InternshipTermId : null));  
		}
	}
}
