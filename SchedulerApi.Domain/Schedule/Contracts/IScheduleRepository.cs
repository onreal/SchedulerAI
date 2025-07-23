namespace SchedulerApi.Domain.Schedule.Contracts
{
    public interface IScheduleRepository
    {
        Task<Schedule> AddAsync(Schedule schedule);
        Task<Schedule?> GetByIdAsync(Guid id);
        Task<IEnumerable<Schedule>> GetAllAsync();
        Task<IEnumerable<Schedule>> GetUpcomingSchedulesAsync();
        Task UpdateAsync(Schedule schedule);
        Task DeleteAsync(Schedule schedule);
    }
}