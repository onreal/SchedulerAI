using SchedulerApi.Application.Schedule.Contracts;
using SchedulerApi.Application.Schedule.DTOs;
using SchedulerApi.Domain.Schedule.Contracts;
using SchedulerApi.Domain.Shared.Contracts;

namespace SchedulerApi.Application.Schedule.Services;

public class UpdateScheduleService(IScheduleRepository repository, IUnitOfWork unitOfWork) : IUpdateScheduleService
{
    public async Task UpdateAsync(Guid id, UpdateScheduleRequest request)
    {
        var schedule = await repository.GetByIdAsync(id);
        if (schedule == null) throw new KeyNotFoundException("Schedule not found.");

        schedule.Update(request.EventTime, request.NotifyBeforeMinutes, request.TemplateId, request.RecipientIds, request.IntegrationIds);
        await repository.UpdateAsync(schedule);
        await unitOfWork.SaveChangesAsync();
    }
}
