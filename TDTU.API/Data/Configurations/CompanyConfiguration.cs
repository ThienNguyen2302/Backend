using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TDTU.API.Data.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
	public void Configure(EntityTypeBuilder<Company> builder)
	{
		builder.HasOne(t => t.User).WithOne(t => t.Company).HasForeignKey<Company>(t => t.Id);
		builder.HasQueryFilter(p => p.DeleteFlag != true);
	}
}
