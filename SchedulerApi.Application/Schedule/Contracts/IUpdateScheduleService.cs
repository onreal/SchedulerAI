using SchedulerApi.Application.Schedule.DTOs;

namespace SchedulerApi.Application.Schedule.Contracts;

public interface IUpdateScheduleService
{
    Task UpdateAsync(Guid id, UpdateScheduleRequest request);
}