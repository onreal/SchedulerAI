namespace SchedulerApi.Application.Schedule.DTOs;

public record CreateScheduleRequest(
    DateTime EventTime,
    int NotifyBeforeMinutes,
    Guid TemplateId,
    List<Guid> RecipientIds,
    List<string> IntegrationIds
);