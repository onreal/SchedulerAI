using SchedulerApi.Domain.UserIntegration;
using SchedulerApi.Infrastructure.Persistence.Accessors;
using SchedulerApi.Infrastructure.Persistence.Entities;

namespace SchedulerApi.Infrastructure.Persistence.Mappers;

public static class UserIntegrationMapper
{
    public static UserIntegration ToDomain(this UserIntegrationEf efUserIntegration)
    {
        if (efUserIntegration == null) return null;
        
        return UserIntegration.Rehydrate(
            efUserIntegration.Id,
            efUserIntegration.UserId,
            efUserIntegration.TemplateId,
            efUserIntegration.Order,
            efUserIntegration.IntegrationId,
            efUserIntegration.Settings);
    }

    public static UserIntegrationEf ToEfEntity(this UserIntegration domainUserIntegration, MappingContextAccessor context)
    {
        if (domainUserIntegration == null) return null;

        var cached = context.Get<UserIntegrationEf>(domainUserIntegration.Id);
        if (cached != null)
        {
            // Update the fields
            cached.UserId = domainUserIntegration.UserId;
            cached.TemplateId = domainUserIntegration.TemplateId;
            cached.Order = domainUserIntegration.Order;
            cached.IntegrationId = domainUserIntegration.IntegrationId;
            cached.Settings = domainUserIntegration.Settings;
            return cached;
        }

        var ef = new UserIntegrationEf
        {
            Id = domainUserIntegration.Id,
            UserId = domainUserIntegration.UserId,
            TemplateId = domainUserIntegration.TemplateId,
            Order = domainUserIntegration.Order,
            IntegrationId = domainUserIntegration.IntegrationId,
            Settings = domainUserIntegration.Settings
        };

        context.Register(domainUserIntegration.Id, ef);
        return ef;
    }
}