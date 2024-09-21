using System.Reflection;

namespace TDTU.API.Data;

public class DataContext : DbContext, IDataContext
{
	public DataContext(DbContextOptions<DataContext> options) : base(options) { }
	public DbSet<Skill> Skills => Set<Skill>();
	public DbSet<Media> Medias => Set<Media>();
	public DbSet<User> Users => Set<User>();
	public DbSet<Role> Roles => Set<Role>();
	public DbSet<Student> Students => Set<Student>();
	public DbSet<Company> Companies => Set<Company>();
	public DbSet<ApplicationStatus> ApplicationStatus => Set<ApplicationStatus>();
	public DbSet<InternshipJob> InternshipJobs => Set<InternshipJob>();
	public DbSet<InternshipJobApplication> InternshipJobApplications => Set<InternshipJobApplication>();
	public DbSet<InternshipRegistration> InternshipRegistrations => Set<InternshipRegistration>();
	public DbSet<RegistrationStatus> RegistrationStatus => Set<RegistrationStatus>();
	public DbSet<InternshipTerm> InternshipTerms => Set<InternshipTerm>();
	public DbSet<RegularJob> RegularJobs => Set<RegularJob>();
	public DbSet<RegularJobApplication> RegularJobApplications => Set<RegularJobApplication>();
	public DbSet<InternshipOrder> InternshipOrders => Set<InternshipOrder>();
	public DbSet<OrderStatus> OrderStatus => Set<OrderStatus>();
	public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();
	public DbSet<Setting> Settings => Set<Setting>();
    public DbSet<StudentSkill> StudentSkills => Set<StudentSkill>();

    public Task<int> SaveChangesAsync()
	{
		return base.SaveChangesAsync();
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}

}
