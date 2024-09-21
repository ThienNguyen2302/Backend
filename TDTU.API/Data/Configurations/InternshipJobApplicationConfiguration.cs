using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TDTU.API.Data.Configurations;

public class InternshipJobApplicationConfiguration : IEntityTypeConfiguration<InternshipJobApplication>
{
	public void Configure(EntityTypeBuilder<InternshipJobApplication> builder)
	{
		builder.HasOne(t => t.Job).WithMany(t => t.Applications).HasForeignKey(t => t.JobId);
		builder.HasOne(t => t.Student).WithMany(t => t.InternshipJobApplications).HasForeignKey(t => t.StudentId);
		builder.HasOne(t => t.Status).WithMany(t => t.InternshipJobApplications).HasForeignKey(t => t.StatusId);
		builder.HasQueryFilter(p => p.DeleteFlag != true);
	}
}
