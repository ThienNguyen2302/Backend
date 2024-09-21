using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TDTU.API.Data.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
	public void Configure(EntityTypeBuilder<Student> builder)
	{
		builder.HasOne(t => t.User).WithOne(t => t.Student).HasForeignKey<Student>(t => t.Id);
		builder.HasQueryFilter(p => p.DeleteFlag != true);
	}
}
