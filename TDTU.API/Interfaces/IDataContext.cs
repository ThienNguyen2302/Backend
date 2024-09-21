namespace TDTU.API.Interfaces;

public interface IDataContext
{
	DbSet<Skill> Skills { get; }
	DbSet<Media> Medias { get; }
	DbSet<User> Users { get; }
	DbSet<Role> Roles { get; }
	DbSet<Student> Students { get; }
	DbSet<Company> Companies { get; }
	DbSet<StudentProfile> StudentProfiles { get; }
	DbSet<InternshipOrder> InternshipOrders { get; }
	DbSet<OrderStatus> OrderStatus { get; }
	DbSet<ApplicationStatus> ApplicationStatus { get; }
	DbSet<InternshipJob> InternshipJobs { get; }
	DbSet<InternshipJobApplication> InternshipJobApplications { get; }
	DbSet<InternshipRegistration> InternshipRegistrations { get; }
	DbSet<RegistrationStatus> RegistrationStatus { get; }
	DbSet<InternshipTerm> InternshipTerms { get; }
	DbSet<RegularJob> RegularJobs { get; }
	DbSet<RegularJobApplication> RegularJobApplications { get; }
	DbSet<Setting> Settings { get; }
    DbSet<StudentSkill> StudentSkills { get; }
    Task<int> SaveChangesAsync();
	Task<int> SaveChangesAsync(CancellationToken token);
	ValueTask DisposeAsync();
}

