using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TDTU.API.Data.Configurations;

public class RegistrationStatusConfiguration : IEntityTypeConfiguration<RegistrationStatus>
{
	public void Configure(EntityTypeBuilder<RegistrationStatus> builder)
	{
		builder.HasQueryFilter(p => p.DeleteFlag != true);
	}
}
