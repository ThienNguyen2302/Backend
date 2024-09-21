using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TDTU.API.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
	public void Configure(EntityTypeBuilder<Role> builder)
	{
		builder.HasQueryFilter(p => p.DeleteFlag != true);
	}
}

