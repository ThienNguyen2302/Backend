namespace TDTU.API.Models.RegularJobModel;

public class RegularJobAddOrUpdate : AddOrUpdateRequest
{
	public Guid? Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public decimal SalaryMin { get; set; }
	public decimal SalaryMax { get; set; }
	public string Description { get; set; } = string.Empty;
	public DateTime ExpireDate { get; set; } = DateTime.Now.AddMonths(1);
	public Guid CompanyId { get; set; }
	public List<Guid> Skills { get; set; }
}
