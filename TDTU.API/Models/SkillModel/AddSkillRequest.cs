namespace TDTU.API.Models.SkillModel;

public class AddSkillRequest
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid ApplicationUserId { get; set; }
}
