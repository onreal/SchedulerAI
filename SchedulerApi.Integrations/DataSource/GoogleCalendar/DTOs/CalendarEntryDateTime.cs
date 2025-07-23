namespace SchedulerApi.Integrations.DataSource.GoogleCalendar.DTOs;

public class CalendarEntryDateTime
{
    public string DateTime { get; set; } = default!;
    public string TimeZone { get; set; } = default!;
}