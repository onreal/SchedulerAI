namespace SchedulerApi.Application.Shared.Contracts;

public interface ISchedulerIntegrationRunner
{
    Task RunAsync(Domain.Schedule.Schedule schedule, CancellationToken cancellationToken = default);
    Task RunPromptAsync(string prompt, CancellationToken cancellationToken = default);
}