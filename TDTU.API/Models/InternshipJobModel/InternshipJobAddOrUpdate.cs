namespace TDTU.API.Models.InternshipJobModel;

public class InternshipJobAddOrUpdate : AddOrUpdateRequest
{
	public Guid? Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public Guid CompanyId { get; set; }
	public Guid InternshipTermId { get; set; }
    public int MaxStudent { get; set; }
    public List<Guid> Skills { get; set; }
}

