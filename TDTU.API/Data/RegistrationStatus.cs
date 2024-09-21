using System.ComponentModel.DataAnnotations.Schema;

namespace TDTU.API.Data;

[Table("tb_registration_status")]
public class RegistrationStatus : BaseStatusEntity
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public ICollection<InternshipRegistration>? Registrations { set; get; }
}

public static class RegistrationStatusConstant
{
	public static string Pending { get; } = nameof(Pending).ToUpper();
	public static string Inprogress { get; } = nameof(Inprogress).ToUpper();
	public static string Done { get; } = nameof(Done).ToUpper();
	public static string Failed { get; } = nameof(Failed).ToUpper();
}
