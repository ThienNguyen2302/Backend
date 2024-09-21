namespace TDTU.API.Models.CompanyModel;

public class CompanyAddOrUpdate : AddOrUpdateRequest
{
	public Guid? Id { get; set; }
	public string Email { get; set; } = string.Empty;
	public string? Phone { get; set; } = string.Empty;
	public string? Address { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public string TaxCode { get; set; } = string.Empty;
	public string Logo { get; set; } = string.Empty;
	public string? Description { get; set; } = string.Empty;
}
