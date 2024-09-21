using System.ComponentModel.DataAnnotations.Schema;

namespace TDTU.API.Data;

[Table("tb_regular_jobs")]
public class RegularJob : BaseEntity
{
	public string Name { get; set; } = string.Empty;
	public decimal SalaryMin { get; set; }
	public decimal SalaryMax { get; set; }
	public string Description { get; set; } = string.Empty;
	public DateTime ExpireDate { get; set; } = DateTime.Now.AddMonths(1);
	public Guid? CompanyId { get; set; }
	public Company? Company { get; set; }
	public ICollection<RegularJobApplication>? Applications { set; get; }
	public ICollection<Skill>? Skills { set; get; }
}
