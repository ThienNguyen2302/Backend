using System.ComponentModel.DataAnnotations.Schema;

namespace TDTU.API.Data;

[Table("tb_student_profiles")]
public class StudentProfile : BaseEntity
{
	public Guid? StudentId { get; set; }
	public Student? Student { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Url { get; set; } = string.Empty;
}
