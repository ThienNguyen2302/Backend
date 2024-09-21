using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TDTU.API.Data.Configurations;

public class StudentProfileConfiguration : IEntityTypeConfiguration<StudentProfile>
{
	public void Configure(EntityTypeBuilder<StudentProfile> builder)
	{
		builder.HasOne(t => t.Student).WithMany(t => t.Profiles).HasForeignKey(t => t.StudentId);
		builder.HasQueryFilter(p => p.DeleteFlag != true);
	}
}
