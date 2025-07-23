using SchedulerApi.Domain.Schedule;
using SchedulerApi.Infrastructure.Persistence.Accessors;
using SchedulerApi.Infrastructure.Persistence.Entities;

namespace SchedulerApi.Infrastructure.Persistence.Mappers;

public static class ScheduleMapper
{
    public static Schedule ToDomain(this ScheduleEf efSchedule)
    {
        if (efSchedule == null) return null;
        
        return Schedule.Rehydrate(
            efSchedule.Id,
            efSchedule.UserId,
            efSchedule.IntegrationIds,
            efSchedule.EventTime,
            efSchedule.NotifyBeforeMinutes,
            efSchedule.RecipientIds,
            efSchedule.IsSent,
            efSchedule.SentAt,
            efSchedule.CreatedAt,
            efSchedule.UpdatedAt);
    }

    public static ScheduleEf ToEfEntity(this Schedule schedule, MappingContextAccessor context)
    {
        if (schedule == null) return null;

        var cached = context.Get<ScheduleEf>(schedule.Id);
        if (cached != null)
        {
            cached.UserId = schedule.UserId;
            cached.IntegrationIds = schedule.IntegrationIds;
            cached.EventTime = schedule.EventTime;
            cached.NotifyBeforeMinutes = schedule.NotifyBeforeMinutes;
            cached.RecipientIds = schedule.RecipientIds;
            cached.IsSent = schedule.IsSent;
            cached.SentAt = schedule.SentAt;
            cached.CreatedAt = schedule.CreatedAt;
            cached.UpdatedAt = schedule.UpdatedAt;
            return cached;
        }

        var ef = new ScheduleEf
        {
            Id = schedule.Id,
            UserId = schedule.UserId,
            IntegrationIds = schedule.IntegrationIds,
            EventTime = schedule.EventTime,
            NotifyBeforeMinutes = schedule.NotifyBeforeMinutes,
            RecipientIds = schedule.RecipientIds,
            IsSent = schedule.IsSent,
            SentAt = schedule.SentAt,
            CreatedAt = schedule.CreatedAt,
            UpdatedAt = schedule.UpdatedAt
        };

        context.Register(schedule.Id, ef);
        return ef;
    }
}