using SchedulerApi.Application.Agents.ScheduleParser.DTOs;

namespace SchedulerApi.Application.Reminder.Contract;

public interface IReminderService
{
    public Task<List<ReminderDto>>  GenerateAsync(ReminderRequest prompt, CancellationToken cancellationToken = default);
}