namespace SchedulerApi.Application.Reminder.DTOs;

public record ReminderAction(string ActionType, string[] TargetNames, string Note);