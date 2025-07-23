namespace SchedulerApi.Integrations.DataSource.GoogleCalendar.DTOs;

public class CalendarEventRecipient
{
    public string Email { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public string ResponseStatus { get; set; } = default!;
}