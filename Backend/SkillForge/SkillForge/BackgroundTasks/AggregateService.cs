using SkillForge.Data;

namespace SkillForge.BackgroundTasks;

public abstract class AggregateService : BackgroundService
{
    protected abstract int FrequencySeconds { get; }

    private readonly IServiceScopeFactory scopeFactory;

    public AggregateService(IServiceScopeFactory scopeFactory)
    {
        this.scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using IServiceScope scope = scopeFactory.CreateScope();
        AppDbContext db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        using PeriodicTimer timer = new(TimeSpan.FromSeconds(FrequencySeconds));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await Aggregate(db);
        }
    }

    public abstract Task Aggregate(AppDbContext db);
}
