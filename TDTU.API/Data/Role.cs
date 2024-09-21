using System.ComponentModel.DataAnnotations.Schema;

namespace TDTU.API.Data;

[Table("tb_roles")]
public class Role : BaseStatusEntity
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public ICollection<User>? Users { set; get; }
}

public static class RoleConstant
{
	public const string Student = "STUDENT";
	public const string Admin = "ADMIN";
	public const string Company = "COMPANY";
}
