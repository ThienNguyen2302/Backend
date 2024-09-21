using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TDTU.API.Data.Configurations;

public class InternshipRegistrationConfiguration : IEntityTypeConfiguration<InternshipRegistration>
{
	public void Configure(EntityTypeBuilder<InternshipRegistration> builder)
	{
		builder.HasOne(t => t.InternshipTerm).WithMany(t => t.Registrations).HasForeignKey(t => t.InternshipTermId);
		builder.HasOne(t => t.Student).WithMany(t => t.Registrations).HasForeignKey(t => t.StudentId);
		builder.HasOne(t => t.Status).WithMany(t => t.Registrations).HasForeignKey(t => t.StatusId);
		builder.HasQueryFilter(p => p.DeleteFlag != true);
	}
}
