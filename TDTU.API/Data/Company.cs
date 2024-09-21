using System.ComponentModel.DataAnnotations.Schema;

namespace TDTU.API.Data;

[Table("tb_companies")]
public class Company : BaseEntity
{
	public string Name { get; set; } = string.Empty;
	public string TaxCode { get; set; } = string.Empty;
	public string Logo { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public User User { get; set; }
	public ICollection<InternshipJob>? InternshipJobs { set; get; }
	public ICollection<RegularJob>? RegularJobs { set; get; }
}
