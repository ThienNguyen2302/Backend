namespace TDTU.API.Data.Seeding;

public class DataContextInitializer : IDataContextInitializer
{
	private readonly IDataContext _context;
	public DataContextInitializer(IDataContext context)
	{
		_context = context;
	}
	public async Task<int> InitRole()
	{
		int rows = 0;
		if(!_context.Roles.Any())
		{
			rows = 1;
		}
		return rows;
	}

	public async Task<int> InitUser()
	{
		int rows = 0;
		if (!_context.Users.Any())
		{
			rows = 1;
		}
		return rows;
	}

	public async Task SeedAsync()
	{
		await InitRole();
		await InitUser();
	}
}
