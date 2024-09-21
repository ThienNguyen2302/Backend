using AutoMapper;

namespace TDTU.API.Dtos.StudentDTO;

public class StudentProfileDto : BaseEntityDto
{
	public Guid? StudentId { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Url { get; set; } = string.Empty;
	public DateTime? CreatedDate { get; set; }
	public DateTime? LastModifiedDate { get; set; }
	private class Mapping : Profile
	{
		public Mapping()
		{
			CreateMap<StudentProfile, StudentProfileDto>();
		}
	}
}
