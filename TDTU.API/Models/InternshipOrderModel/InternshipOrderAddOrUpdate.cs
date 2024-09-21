namespace TDTU.API.Models.InternshipOrderModel;

public class InternshipOrderAddOrUpdate : AddOrUpdateRequest
{
	public Guid? Id { get; set; }
	public Guid StudentId { get; set; }
	public Guid RegistrationId { get; set; }
	public string Company { get; set; } = string.Empty;
	public string TaxCode { get; set; } = string.Empty;
	public string Position { get; set; } = string.Empty;
	public DateTime StartDate { get; set; } = DateTime.Now;
	public DateTime EndDate { get; set; } = DateTime.Now.AddMonths(3);
}
