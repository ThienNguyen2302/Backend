using System.ComponentModel.DataAnnotations.Schema;

namespace TDTU.API.Data;

[Table("tb_internship_registrations")]
public class InternshipRegistration : BaseEntity
{
	public Guid? InternshipTermId { get; set; }
	public InternshipTerm? InternshipTerm { get; set; }
	public Guid? StudentId { get; set; }
	public Student? Student { get; set; }
	public string? StatusId { get; set; }
	public RegistrationStatus? Status { get; set; }
	public ICollection<InternshipOrder>? Orders { set; get; }
	public bool IsExpired { get; set; } = false;
}
