using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TDTU.API.Data.Configurations;

public class InternshipOrderConfiguration : IEntityTypeConfiguration<InternshipOrder>
{
	public void Configure(EntityTypeBuilder<InternshipOrder> builder)
	{
		builder.HasOne(t => t.Registration).WithMany(t => t.Orders).HasForeignKey(t => t.RegistrationId);
		builder.HasOne(t => t.Status).WithMany(t => t.Orders).HasForeignKey(t => t.StatusId);
		builder.HasQueryFilter(p => p.DeleteFlag != true);
	}
}
