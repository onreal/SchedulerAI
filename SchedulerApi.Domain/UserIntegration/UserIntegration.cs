using SchedulerApi.Domain.Shared;

namespace SchedulerApi.Domain.UserIntegration;

public class UserIntegration : Entity
{
    public Guid UserId { get; private set; }
    public Guid TemplateId { get; private set; }
    public int Order { get; private set; }
    public string IntegrationId { get; private set; }
    public string Settings { get; private set; }

    public UserIntegration(Guid userId, Guid templateId, int order, string integrationId, string settings)
    {
        if (string.IsNullOrWhiteSpace(integrationId))
            throw new ArgumentException("Type cannot be empty", nameof(integrationId));

        if (string.IsNullOrWhiteSpace(settings))
            throw new ArgumentException("Settings cannot be empty", nameof(settings));

        TemplateId = templateId;
        Order = order;
        IntegrationId = integrationId;
        UserId = userId;
        Settings = settings;
    }
    
    public static UserIntegration Create(Guid templateId, int order, string integrationId, string settings)
    {
        return new UserIntegration(Guid.Empty, templateId, order, integrationId, settings);
    }

    public void Update(string type, string settings)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Type cannot be empty", nameof(type));

        if (string.IsNullOrWhiteSpace(settings))
            throw new ArgumentException("Settings cannot be empty", nameof(settings));

        IntegrationId = type;
        Settings = settings;
    }
    
    public static UserIntegration Rehydrate(
        Guid id,
        Guid userId,
        Guid templateId,
        int order,
        string integrationId,
        string settings
    )
    {
        var userIntegration = new UserIntegration(
            userId,
            templateId,
            order,
            integrationId,
            settings
        );
        userIntegration.SetId(id);

        return userIntegration;
    }
}