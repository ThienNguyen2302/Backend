using System.ComponentModel.DataAnnotations.Schema;

namespace TDTU.API.Data;

[Table("tb_application_status")]
public class ApplicationStatus : BaseStatusEntity
{
	public string Name { get; set; }
	public string Description { get; set; }
	public ICollection<InternshipJobApplication>? InternshipJobApplications { set; get; }
}

public static class ApplicationStatusConstant
{
	public static string Declined { get; } = nameof(Declined).ToUpper();
	public static string Pending { get; } = nameof(Pending).ToUpper();
	public static string Accepted { get; } = nameof(Accepted).ToUpper();
	public static string Offered { get; } = nameof(Offered).ToUpper();
	public static string Interning { get; } = nameof(Interning).ToUpper();
}
