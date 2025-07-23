using SchedulerApi.Application.Schedule.DTOs;

namespace SchedulerApi.Application.Schedule.Contracts;

public interface IGetScheduleService
{
    Task<IEnumerable<GetSchedule>> GetAllAsync();
    Task<GetSchedule?> GetByIdAsync(Guid id);
}