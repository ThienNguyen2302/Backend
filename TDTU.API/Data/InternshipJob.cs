using System.ComponentModel.DataAnnotations.Schema;

namespace TDTU.API.Data;

[Table("tb_internship_jobs")]
public class InternshipJob : BaseEntity
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public int MaxStudent {  get; set; }
	public Guid? CompanyId { get; set; }
	public Company? Company { get; set; }
	public Guid? InternshipTermId { get; set; }
	public InternshipTerm? InternshipTerm { get; set; }
	public ICollection<InternshipJobApplication>? Applications { set; get; }
	public ICollection<Skill>? Skills { set; get; }
}
