using System.ComponentModel.DataAnnotations.Schema;

namespace TDTU.API.Data;

[Table("tb_regular_job_applications")]
public class RegularJobApplication : BaseEntity
{
	public Guid? JobId { get; set; }
	public RegularJob? Job { get; set; }
	public Guid? StudentId { get; set; }
	public Student? Student { get; set; }
	public string FullName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public string CV { get; set; } = string.Empty;
	public string Introduce { get; set; } = string.Empty;
	public string? Company { get; set; } = string.Empty;
	public string? Position { get; set; } = string.Empty;
	public decimal? SalaryMin { get; set; }
	public decimal? SalaryMax { get; set; }
}
