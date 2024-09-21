using System.ComponentModel.DataAnnotations.Schema;

namespace TDTU.API.Data;

[Table("tb_internship_terms")]
public class InternshipTerm : BaseEntity
{
	public string Name { get; set; } = string.Empty;
	public DateTime StartDate { get; set; } = DateTime.Now;
	public DateTime EndDate { get; set; } = DateTime.Now.AddMonths(3);
	public bool IsExpired { get; set; } = false;
	public ICollection<InternshipRegistration>? Registrations { set; get; }
	public ICollection<InternshipJob>? Jobs { set; get; }
}


