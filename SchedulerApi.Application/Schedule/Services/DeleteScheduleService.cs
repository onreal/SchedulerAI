using SchedulerApi.Application.Schedule.Contracts;
using SchedulerApi.Domain.Schedule.Contracts;
using SchedulerApi.Domain.Shared.Contracts;

namespace SchedulerApi.Application.Schedule.Services;

public class DeleteScheduleService(IScheduleRepository repository, IUnitOfWork unitOfWork) : IDeleteScheduleService
{
    public async Task DeleteAsync(Guid id)
    {
        var schedule = await repository.GetByIdAsync(id);
        if (schedule == null) throw new KeyNotFoundException("Schedule not found.");

        await repository.DeleteAsync(schedule);
        await unitOfWork.SaveChangesAsync();
    }
}