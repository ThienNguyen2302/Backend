using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TDTU.API.Data.Configurations;

public class RegularJobConfiguration : IEntityTypeConfiguration<RegularJob>
{
	public void Configure(EntityTypeBuilder<RegularJob> builder)
	{
		builder.HasOne(t => t.Company).WithMany(t => t.RegularJobs).HasForeignKey(t => t.CompanyId);
		builder.HasQueryFilter(p => p.DeleteFlag != true);
	}
}
