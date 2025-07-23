using SchedulerApi.Application.Exceptions;
using SchedulerApi.Application.Shared.Contracts;
using SchedulerApi.Application.Tasks.Contracts;
using SchedulerApi.Domain.Schedule.Contracts;

namespace SchedulerApi.Application.Tasks.Services;

public class TaskService(ISchedulerIntegrationRunner integrationRunner, IScheduleRepository scheduleRepository) : ITaskService
{
    public async Task RunAsync(Guid scheduleId)
    {
        var schedule = await scheduleRepository.GetByIdAsync(scheduleId);
        if (schedule == null)
            throw new NotFoundException("Schedule not found");

        await integrationRunner.RunAsync(schedule);
    }

    public async Task RunPromptAsync(string prompt)
    {
        await integrationRunner.RunPromptAsync(prompt);
    }
}