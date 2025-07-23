namespace SchedulerApi.Application.Schedule.DTOs;

public record UpdateScheduleRequest(
    DateTime EventTime,
    int NotifyBeforeMinutes,
    Guid TemplateId,
    List<Guid> RecipientIds,
    List<string> IntegrationIds
);