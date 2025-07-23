using SchedulerApi.Application.Schedule.Contracts;
using SchedulerApi.Application.Schedule.DTOs;
using SchedulerApi.Application.Shared.Contracts;
using SchedulerApi.Domain.Schedule.Contracts;

namespace SchedulerApi.Application.Schedule.Services;

public class GetScheduleService(IScheduleRepository repository, ICurrentUserAccessor currentUser)
    : IGetScheduleService
{
    private readonly ICurrentUserAccessor _currentUser = currentUser;

    public async Task<IEnumerable<GetSchedule>> GetAllAsync()
    {
        var list = await repository.GetAllAsync();
        return list.Select(GetSchedule.FromDomain);
    }

    public async Task<GetSchedule?> GetByIdAsync(Guid id)
    {
        var schedule = await repository.GetByIdAsync(id);
        return schedule == null ? null : GetSchedule.FromDomain(schedule);
    }
}