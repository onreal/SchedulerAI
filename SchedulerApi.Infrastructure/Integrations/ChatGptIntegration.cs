using SchedulerApi.Domain.Schedule;

namespace SchedulerApi.Infrastructure.Integrations;

public class ChatGptIntegration
{
    public string Name => "ChatGPT";

    public async Task HandleAsync(Schedule schedule, CancellationToken cancellationToken = default)
    {
        // Use Google API SDK to create event
        // Transform domain schedule into Google Calendar event
        // Handle token access, refresh logic, etc.
    }
}