using SchedulerApi.Application.Shared.Contracts;
using SchedulerApi.Domain.Schedule.Contracts;
using SchedulerApi.Domain.User.Contracts;
using SchedulerApi.Infrastructure.Persistence;

namespace SchedulerApi.Worker;

public class SchedulerWorker : BackgroundService
{
    private readonly ILogger<SchedulerWorker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public SchedulerWorker(ILogger<SchedulerWorker> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _logger.LogInformation("Started...");
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Started...");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<SchedulerDbContext>();
                var currentUserAccessor = scope.ServiceProvider.GetRequiredService<ICurrentUserAccessor>();
                var integrationRunner = scope.ServiceProvider.GetRequiredService<ISchedulerIntegrationRunner>();
                var scheduleRepository = scope.ServiceProvider.GetRequiredService<IScheduleRepository>();
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                var schedules = await scheduleRepository.GetUpcomingSchedulesAsync();
                
                foreach (var schedule in schedules.ToList())
                {
                    var user = await userRepository.GetByIdAsync(schedule.UserId);
                    if (user == null) {
                        _logger.LogError("User not found for schedule {Id}. Skipping execution", schedule.Id);
                        continue;
                    }
                    
                    currentUserAccessor.SetUser(user);
                    _logger.LogInformation("Executing schedule {Id}", schedule.Id);
                    await integrationRunner.RunAsync(schedule, stoppingToken);

                    schedule.MarkAsSent();
                    await scheduleRepository.UpdateAsync(schedule);
                }

                await dbContext.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.LogError(ex, "Failed executing schedules");
            }
            Console.WriteLine("Finished");
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}