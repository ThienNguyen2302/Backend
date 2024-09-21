using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TDTU.API.Data.Configurations;

public class SkillConfiguration : IEntityTypeConfiguration<Skill>
{
	public void Configure(EntityTypeBuilder<Skill> builder)
	{
		builder.HasMany(t => t.RegularJobs).WithMany(t => t.Skills);
		builder.HasMany(t => t.InternshipJobs).WithMany(t => t.Skills);
		builder.HasQueryFilter(p => p.DeleteFlag != true);
	}
}
