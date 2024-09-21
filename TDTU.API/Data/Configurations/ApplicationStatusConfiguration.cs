using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TDTU.API.Data.Configurations;

public class ApplicationStatusConfiguration : IEntityTypeConfiguration<ApplicationStatus>
{
	public void Configure(EntityTypeBuilder<ApplicationStatus> builder)
	{
		builder.HasQueryFilter(p => p.DeleteFlag != true);
	}
}
