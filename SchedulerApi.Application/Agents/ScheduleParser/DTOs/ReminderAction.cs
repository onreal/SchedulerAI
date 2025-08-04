namespace SchedulerApi.Application.Agents.ScheduleParser.DTOs;

public record ReminderAction(string ActionType, string[] TargetNames, string Note);