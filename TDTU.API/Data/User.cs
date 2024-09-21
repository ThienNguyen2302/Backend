using System.ComponentModel.DataAnnotations.Schema;

namespace TDTU.API.Data;

[Table("tb_users")]
public class User : BaseEntity
{
	public string Email { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public string Address { get; set; } = string.Empty;
	public string? RoleId { get; set; }
	public Role? Role { get; set; }
	public Student? Student { get; set; }
	public Company? Company { get; set; }
}
