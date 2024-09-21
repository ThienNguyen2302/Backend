namespace TDTU.API.BackgroundServices;

public class InternshipTermBackgroundService : BackgroundService
{
	private readonly IServiceScopeFactory _serviceProvider;
	private readonly TimeSpan _delay = TimeSpan.FromHours(1);
	public InternshipTermBackgroundService(IServiceScopeFactory serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			using (var scope = _serviceProvider.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<IDataContext>();
				try
				{
					await ExpireTerm(context, stoppingToken);
					await context.DisposeAsync();
					scope.Dispose();
				}
				catch (Exception ex)
				{
					Console.WriteLine($"[Failed] - [BackgroundService] - exception: {ex.Message}");
				}
			}
			await Task.Delay(_delay, stoppingToken);
		}
	}

	private async Task ExpireTerm(IDataContext context, CancellationToken token)
	{
		DateTime now = DateTime.Now;
		var terms = await context.InternshipTerms.Include(s => s.Registrations)
								 .Where(s => s.EndDate < now && s.IsExpired != true)
								 .ToListAsync();
		if (terms.Any())
		{
			foreach(var term in terms)
			{
				term.IsExpired = true;
				term.LastModifiedDate = now;

				if(term.Registrations != null && term.Registrations.Any())
				{
					foreach (var registration in term.Registrations)
					{
						if(registration.StatusId == RegistrationStatusConstant.Pending)
						{
							registration.StatusId = RegistrationStatusConstant.Failed;
							
						}
						if (registration.StatusId == RegistrationStatusConstant.Inprogress)
						{
							registration.StatusId = RegistrationStatusConstant.Done;
						}
						registration.IsExpired = true;
						registration.LastModifiedDate = now;
					}
					context.InternshipRegistrations.UpdateRange(term.Registrations);
				}
			}
			context.InternshipTerms.UpdateRange(terms);
			var rows = await context.SaveChangesAsync(token);
			Console.WriteLine($"[Success] - [BackgroundService] - has expires {rows} item");
		}
	}
}
