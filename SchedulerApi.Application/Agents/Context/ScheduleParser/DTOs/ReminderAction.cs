namespace SchedulerApi.Application.Agents.Context.ScheduleParser.DTOs;

public record ReminderAction(string ActionType, string[] TargetNames, string Note);