namespace SchedulerApi.Application.Integrations.Contracts;

public interface ITransactionalIntegrationServices : IIntegrationService
{
    Task SendMessageAsync(string toPhoneNumber, string message, CancellationToken cancellationToken = default);
}