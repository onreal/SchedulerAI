using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SchedulerApi.Domain.UserIntegration;
using SchedulerApi.Integrations.DataSource.GoogleCalendar.Contracts;
using SchedulerApi.Integrations.DataSource.GoogleCalendar.DTOs;

namespace SchedulerApi.Integrations.DataSource.GoogleCalendar.Services;

public class GoogleCalendarIntegrationService : IGoogleCalendarIntegrationService
{
    public string Name => "GoogleCalendar";
    private CalendarService service;

    public async Task HandleAsync(
        UserIntegration? userIntegration,
        IConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        var settings = JsonConvert.DeserializeObject<GoogleCalendarOptions>(
            (userIntegration != null 
                ? userIntegration.Settings 
                : configuration.GetSection("Integrations:GoogleCalendar").Value) ?? string.Empty);

        if (settings == null)
        {
            throw new ArgumentNullException(nameof(settings), "Google Calendar settings cannot be null.");
        }
        
        var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            new ClientSecrets
            {
                ClientId = settings.ClientId,
                ClientSecret = settings.ClientSecret
            },
            new[] { CalendarService.Scope.CalendarReadonly },
            userIntegration.UserId.ToString(),
            cancellationToken);

        service = new CalendarService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "SchedulerApi"
        });
    }

    public async Task<IList<CalendarListEntry>> FetchUserCalendarsAsync(string accessToken,
        CancellationToken cancellationToken = default)
    {
        var calendarListRequest = service.CalendarList.List();
        CalendarList calendarList = await calendarListRequest.ExecuteAsync(cancellationToken);

        return calendarList.Items;
    }

    public async Task<IList<Event>> FetchCalendarDetailsAsync(string calendarId, string accessToken)
    {
        var eventsRequest = service.Events.List(calendarId);
        eventsRequest.ShowDeleted = false;
        eventsRequest.SingleEvents = true;

        var events = await eventsRequest.ExecuteAsync();

        return events.Items;
    }
}