using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TDTU.API.Data.Configurations;

public class InternshipTermConfiguration : IEntityTypeConfiguration<InternshipTerm>
{
	public void Configure(EntityTypeBuilder<InternshipTerm> builder)
	{
		builder.HasQueryFilter(p => p.DeleteFlag != true);
	}
}
