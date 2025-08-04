using SchedulerApi.Application.Agents.Context.ScheduleParser.DTOs;

namespace SchedulerApi.Application.Reminder.Contract;

public interface IReminderService
{
    public Task<List<ReminderDto>>  GenerateAsync(ReminderRequest prompt, CancellationToken cancellationToken = default);
}