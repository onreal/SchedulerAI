namespace SchedulerApi.Application.Schedule.Contracts;

public interface IDeleteScheduleService
{
    Task DeleteAsync(Guid id);
}