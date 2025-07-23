namespace SchedulerApi.Application.Schedule.DTOs;

public record GetSchedule(
    Guid Id,
    DateTime EventTime,
    int NotifyBeforeMinutes,
    List<Guid> RecipientIds,
    List<string> IntegrationIds,
    bool IsSent,
    DateTime? SentAt
)
{
    public static GetSchedule FromDomain(Domain.Schedule.Schedule schedule) => new(
        schedule.Id,
        schedule.EventTime,
        schedule.NotifyBeforeMinutes,
        schedule.RecipientIds,
        schedule.IntegrationIds ?? new List<string>(),
        schedule.IsSent,
        schedule.SentAt
    );
}