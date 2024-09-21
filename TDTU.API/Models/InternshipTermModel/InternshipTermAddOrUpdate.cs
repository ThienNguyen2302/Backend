namespace TDTU.API.Models.InternshipTermModel;

public class InternshipTermAddOrUpdate : AddOrUpdateRequest
{
	public Guid? Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public DateTime StartDate { get; set; } = DateTime.Now;
	public DateTime EndDate { get; set; } = DateTime.Now.AddMonths(3);
}
