using Microsoft.Extensions.Configuration;

namespace SchedulerApi.Application.Integrations.Contracts;

public interface IIntegrationService
{
    string Name { get; }

    Task HandleAsync(Domain.UserIntegration.UserIntegration? userIntegration,
        IConfiguration configuration,
        CancellationToken cancellationToken = default);
}