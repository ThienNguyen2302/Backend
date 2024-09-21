using System.ComponentModel.DataAnnotations.Schema;

namespace TDTU.API.Data;

[Table("tb_students")]
public class Student : BaseEntity
{
	public string FullName { get; set; } = string.Empty;
	public string Code { get; set; } = string.Empty;
	public DateTime? StartDate { get; set; } = DateTime.Now.AddYears(-4);
	public string? Major { get; set; } = string.Empty;
    public string Introduction { get; set; } = string.Empty;
    public string Skill { get; set; } = string.Empty;
    public User User { get; set; }
	public ICollection<StudentProfile>? Profiles { set; get; }
	public ICollection<InternshipRegistration>? Registrations { set; get; }
	public ICollection<RegularJobApplication>? RegularJobApplications { set; get; }
	public ICollection<InternshipJobApplication>? InternshipJobApplications { set; get; }
    public ICollection<StudentSkill>? StudentSkills { set; get; }
}
