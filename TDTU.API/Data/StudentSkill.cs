using System.ComponentModel.DataAnnotations.Schema;

namespace TDTU.API.Data;

[Table("tb_student_skills")]
public class StudentSkill : BaseEntity
{
    public Guid? StudentId { get; set; }
    public Student? Student { get; set; }
    public Guid? SkillId { get; set; }
    public Skill? Skill { get; set; }
}
