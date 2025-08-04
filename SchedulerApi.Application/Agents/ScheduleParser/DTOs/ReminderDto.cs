namespace SchedulerApi.Application.Agents.ScheduleParser.DTOs;

public record ReminderDto(
    string[]? Names,
    DateTime? EventDateTime,
    string? Location,
    DateTime?
        RemindBefore,
    ReminderAction[] Actions,
    bool IsSufficient,
    string? Reason
);