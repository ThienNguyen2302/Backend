using System.ComponentModel.DataAnnotations.Schema;

namespace TDTU.API.Data;

[Table("tb_settings")]
public class Setting : BaseEntity
{
	public int NumberInternshipJob { get; set; }
}
