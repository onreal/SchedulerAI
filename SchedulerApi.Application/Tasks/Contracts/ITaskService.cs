namespace SchedulerApi.Application.Tasks.Contracts;

public interface ITaskService
{
    public Task RunAsync(Guid scheduleId);
    public Task RunPromptAsync(string prompt);
}