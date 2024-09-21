using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TDTU.API.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.HasOne(t => t.Role).WithMany(t => t.Users).HasForeignKey(t => t.RoleId);
		builder.HasQueryFilter(p => p.DeleteFlag != true);
	}
}
