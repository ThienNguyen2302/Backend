using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TDTU.API.Data.Configurations;

public class InternshipJobConfiguration : IEntityTypeConfiguration<InternshipJob>
{
	public void Configure(EntityTypeBuilder<InternshipJob> builder)
	{
		builder.HasOne(t => t.Company).WithMany(t => t.InternshipJobs).HasForeignKey(t => t.CompanyId);
		builder.HasOne(t => t.InternshipTerm).WithMany(t => t.Jobs).HasForeignKey(t => t.InternshipTermId);
		builder.HasQueryFilter(p => p.DeleteFlag != true);
	}
}
