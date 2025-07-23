using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Calendar.v3.Data;
using SchedulerApi.Application.Integrations.Contracts;

namespace SchedulerApi.Integrations.DataSource.GoogleCalendar.Contracts;

public interface IGoogleCalendarIntegrationService : IIntegrationService
{
    Task<IList<CalendarListEntry>> FetchUserCalendarsAsync(string accessToken,
        CancellationToken cancellationToken = default);

    Task<IList<Event>> FetchCalendarDetailsAsync(string calendarId, string accessToken);
}