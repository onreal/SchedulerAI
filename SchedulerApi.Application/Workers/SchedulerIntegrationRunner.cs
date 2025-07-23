using Microsoft.Extensions.Configuration;
using SchedulerApi.Application.Integrations.Contracts;
using SchedulerApi.Application.Shared.Contracts;
using SchedulerApi.Domain.Recipient.Contracts;
using SchedulerApi.Domain.Template.Contracts;
using SchedulerApi.Domain.UserIntegration.Contracts;
using SchedulerApi.Domain.ValueObjects;

namespace SchedulerApi.Application.Workers;

public class SchedulerIntegrationRunner(
    IEnumerable<IIntegrationService> integrationServices,
    IConfiguration configuration,
    IUserIntegrationRepository userIntegrationRepository,
    ITemplateRepository templateRepository,
    IRecipientRepository recipientRepository)
    : ISchedulerIntegrationRunner
{
    public async Task RunAsync(Domain.Schedule.Schedule schedule, CancellationToken cancellationToken)
    {
        var recipients = await GetRecipients(schedule.RecipientIds, cancellationToken);
        var integrations = schedule.IntegrationIds.Select(Integration.FromName)
            .OrderBy(x => x.Order)
            .ToList();

        var recipientOutputs = recipients.ToDictionary(r => r.Id, _ => (string?)null);

        foreach (var integration in integrations)
        {
            var userIntegrations = await GetUserIntegrations(integration.Name, cancellationToken);

            foreach (var userIntegration in userIntegrations)
            {
                var template = await GetTemplate(userIntegration.TemplateId, cancellationToken);
                var service = GetIntegrationService(integration.Name);

                await service.HandleAsync(userIntegration, configuration, cancellationToken);

                if (service is IGenerativeIntegrationServices generative)
                {
                    await ProcessGenerativeAsync(
                        generative,
                        template.Message,
                        schedule,
                        recipients,
                        recipientOutputs,
                        cancellationToken);
                }
                else if (service is ITransactionalIntegrationServices transactional)
                {
                    await ProcessTransactionalAsync(
                        transactional, 
                        template.Message, 
                        schedule, 
                        recipients, 
                        recipientOutputs, 
                        cancellationToken);
                }
            }
        }
    }

    public Task RunPromptAsync(string prompt, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private async Task<List<Domain.Recipient.Recipient>> GetRecipients(List<Guid> recipientIds, CancellationToken cancellationToken)
    {
        var recipients = (await recipientRepository.GetByIdsAsync(recipientIds)).ToList();
        if (recipients.Count == 0)
            throw new InvalidOperationException("No recipients found for transactional message.");
        return recipients;
    }

    private async Task<List<Domain.UserIntegration.UserIntegration>> GetUserIntegrations(string integrationName, CancellationToken cancellationToken)
    {
        var userIntegrations = await userIntegrationRepository.GetByIntegrationAsync(integrationName);
        return userIntegrations.OrderBy(x => x.Order).ToList();
    }

    private async Task<Domain.Template.Template> GetTemplate(Guid templateId, CancellationToken cancellationToken)
    {
        var template = await templateRepository.GetByIdAsync(templateId);
        if (template is null)
            throw new KeyNotFoundException($"Template with ID {templateId} not found.");
        return template;
    }

    private IIntegrationService GetIntegrationService(string integrationName)
    {
        return integrationServices.First(x => x.Name == integrationName);
    }

    private async Task ProcessGenerativeAsync(
        IGenerativeIntegrationServices generative,
        string templateMessage,
        Domain.Schedule.Schedule schedule,
        List<Domain.Recipient.Recipient> recipients,
        Dictionary<Guid, string?> recipientOutputs,
        CancellationToken cancellationToken)
    {
        foreach (var recipient in recipients)
        {
            var message = BuildPersonalizedMessage(templateMessage, schedule, recipient);
            var output = await generative.GenerateMessageAsync<string>(message, cancellationToken);
            recipientOutputs[recipient.Id] = output;
        }
    }

    private async Task ProcessTransactionalAsync(
        ITransactionalIntegrationServices transactional,
        string templateMessage,
        Domain.Schedule.Schedule schedule,
        List<Domain.Recipient.Recipient> recipients,
        Dictionary<Guid, string?> recipientOutputs,
        CancellationToken cancellationToken)
    {
        foreach (var recipient in recipients)
        {
            var message = recipientOutputs[recipient.Id] 
                          ?? BuildPersonalizedMessage(templateMessage, schedule, recipient);
            await transactional.SendMessageAsync(recipient.Phone, message, cancellationToken);
        }
    }

    private string BuildPersonalizedMessage(string templateMessage, Domain.Schedule.Schedule schedule, Domain.Recipient.Recipient recipient, string? previousResponse = null)
    {
        var placeholders = new Dictionary<string, string>
        {
            ["{name}"] = recipient.Name,
            ["{surname}"] = recipient.Surname,
            ["{email}"] = recipient.Email,
            ["{phone}"] = recipient.Phone,
            ["{event_date}"] = schedule.EventTime.ToString("yyyy-MM-dd HH:mm"),
            ["{previous_response}"] = previousResponse ?? string.Empty
        };

        foreach (var (key, value) in placeholders)
        {
            templateMessage = templateMessage.Replace(key, value ?? string.Empty);
        }

        return templateMessage;
    }
}