using SchedulerApi.Application.Agents.Context.ScheduleParser.DTOs;
using SchedulerApi.Application.Agents.Implementation;

namespace SchedulerApi.Application.Reminder.Contract;

public interface IReminderService
{
    public Task<AgentResponse>  GenerateAsync(ReminderRequest prompt, CancellationToken cancellationToken = default);
}