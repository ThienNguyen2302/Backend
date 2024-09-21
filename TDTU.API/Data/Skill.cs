using System.ComponentModel.DataAnnotations.Schema;

namespace TDTU.API.Data;

[Table("tb_skills")]
public class Skill : BaseEntity
{
	public string Name { get; set; }
	public string Description { get; set; }
	public string Sort { get; set; }
	public ICollection<RegularJob>? RegularJobs { set; get; }
	public ICollection<InternshipJob>? InternshipJobs { set; get; }
    public ICollection<StudentSkill>? StudentSkills { set; get; }
}
