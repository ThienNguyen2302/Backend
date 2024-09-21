using System.ComponentModel.DataAnnotations.Schema;

namespace TDTU.API.Data;

[Table("tb_internship_orders")]
public class InternshipOrder : BaseEntity
{
	public Guid? RegistrationId { get; set; }
	public InternshipRegistration? Registration { get; set; }
	public string? StatusId { get; set; }
	public OrderStatus? Status { get; set; }
	public string Company { get; set; } = string.Empty;
	public string TaxCode { get; set; } = string.Empty;
	public string Position { get; set; } = string.Empty;
	public bool IsAttach { get; set; } = false;
	public DateTime StartDate { get; set; } = DateTime.Now;
	public DateTime EndDate { get; set; } = DateTime.Now.AddMonths(3);
}
