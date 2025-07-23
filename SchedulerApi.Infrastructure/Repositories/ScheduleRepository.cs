using Microsoft.EntityFrameworkCore;
using SchedulerApi.Domain.Schedule;
using SchedulerApi.Domain.Schedule.Contracts;
using SchedulerApi.Infrastructure.Persistence;
using SchedulerApi.Infrastructure.Persistence.Accessors;
using SchedulerApi.Infrastructure.Persistence.Mappers;

namespace SchedulerApi.Infrastructure.Repositories;

public class ScheduleRepository : IScheduleRepository
{
    private readonly SchedulerDbContext _context;
    private readonly MappingContextAccessor _mappingContextAccessor;

    public ScheduleRepository(SchedulerDbContext context, MappingContextAccessor mappingContextAccessor)
    {
        _context = context;
        _mappingContextAccessor = mappingContextAccessor;
    }

    public async Task<Schedule?> GetByIdAsync(Guid id)
    {
        var ef = await _context.Schedules.FindAsync(id);
        if (ef == null) return null;

        _mappingContextAccessor.Register(ef.Id, ef);
        return ef.ToDomain();
    }

    public async Task<IEnumerable<Schedule>> GetAllAsync()
    {
        var efList = await _context.Schedules
            .ToListAsync();

        foreach (var ef in efList)
        {
            _mappingContextAccessor.Register(ef.Id, ef);
        }

        return efList.Select(s => s.ToDomain());
    }

    public async Task<IEnumerable<Schedule>> GetUpcomingSchedulesAsync()
    {
        var now = DateTime.UtcNow;
        
        var efList = await _context.Schedules
            .Where(x => !x.IsSent && x.EventTime <= now.AddMinutes(44450)).ToListAsync();
        
        foreach (var ef in efList)
        {
            _mappingContextAccessor.Register(ef.Id, ef);
        }
        
        return efList.Select(s => s.ToDomain());
    }

    public async Task<Schedule> AddAsync(Schedule schedule)
    {
        var ef = schedule.ToEfEntity(_mappingContextAccessor);
        _mappingContextAccessor.Register(ef.Id, ef);
        await _context.Schedules.AddAsync(ef);

        return ef.ToDomain();
    }

    public Task UpdateAsync(Schedule schedule)
    {
        var ef = schedule.ToEfEntity(_mappingContextAccessor);
        _mappingContextAccessor.Register(ef.Id, ef);
        _context.Schedules.Update(ef);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Schedule schedule)
    {
        var ef = schedule.ToEfEntity(_mappingContextAccessor);
        _mappingContextAccessor.Register(ef.Id, ef);
        _context.Schedules.Remove(ef);
        return Task.CompletedTask;
    }
}
