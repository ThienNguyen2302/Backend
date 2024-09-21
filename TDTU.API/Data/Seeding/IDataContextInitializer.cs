namespace TDTU.API.Data.Seeding;

public interface IDataContextInitializer
{
	Task SeedAsync();
	Task<int> InitRole();
	Task<int> InitUser();
}