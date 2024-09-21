using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TDTU.API.Data.Configurations;

public class RegularJobApplicationConfiguration : IEntityTypeConfiguration<RegularJobApplication>
{
	public void Configure(EntityTypeBuilder<RegularJobApplication> builder)
	{
		builder.HasOne(t => t.Job).WithMany(t => t.Applications).HasForeignKey(t => t.JobId);
		builder.HasOne(t => t.Student).WithMany(t => t.RegularJobApplications).HasForeignKey(t => t.StudentId);
		builder.HasQueryFilter(p => p.DeleteFlag != true);
	}
}
