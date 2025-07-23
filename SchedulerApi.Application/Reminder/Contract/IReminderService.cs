using SchedulerApi.Application.Reminder.DTOs;

namespace SchedulerApi.Application.Reminder.Contract;

public interface IReminderService
{
    public Task<List<ReminderDto>>  GenerateAsync(ReminderRequest prompt, CancellationToken cancellationToken = default);
    public Task SendMessageAsync(string message, CancellationToken cancellationToken = default);

    public Task PerformActionAsync(ReminderRequest prompt, CancellationToken cancellationToken = default);
}