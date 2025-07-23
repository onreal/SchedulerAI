using System.Collections.Generic;

namespace SchedulerApi.Integrations.DataSource.GoogleCalendar.DTOs;

public class GoogleCalendarResponse
{
    public string Id { get; set; } = default!;
    public string Summary { get; set; } = default!;
    public string Timezone { get; set; } = default!;
    public CalendarEntryDateTime Start { get; set; } = default!;
    public CalendarEntryDateTime End { get; set; } = default!;
    public CalendarEventCreator Creator { get; set; } = default!;
    public List<CalendarEventRecipient>? Attendees { get; set; } = new();
}