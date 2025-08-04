namespace SchedulerApi.Application.Integrations.Contracts;

public interface IGenerativeIntegrationServices : IIntegrationService
{
    Task<T> GenerateMessageAsync<T>(string prompt,
        CancellationToken cancellationToken = default);

    void SetSystemMessages(string chatMessage);

    void ClearUserMessages();
}