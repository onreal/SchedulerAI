using SchedulerApi.Domain.Template;
using SchedulerApi.Infrastructure.Persistence.Accessors;
using SchedulerApi.Infrastructure.Persistence.Entities;

namespace SchedulerApi.Infrastructure.Persistence.Mappers;

public static class TemplateMapper
{
    public static Template ToDomain(this TemplateEf efTemplate)
    {
        if (efTemplate == null) return null;
        
        return Template.Rehydrate(
            efTemplate.Id,
            efTemplate.UserId,
            efTemplate.Message,
            efTemplate.CreatedAt,
            efTemplate.UpdatedAt);
    }

    public static TemplateEf ToEfEntity(this Template domainTemplate, MappingContextAccessor context)
    {
        if (domainTemplate == null) return null;

        var cached = context.Get<TemplateEf>(domainTemplate.Id);
        if (cached != null)
        {
            cached.UserId = domainTemplate.UserId;
            cached.Message = domainTemplate.Message;
            cached.CreatedAt = domainTemplate.CreatedAt;
            cached.UpdatedAt = domainTemplate.UpdatedAt;
            return cached;
        }

        var ef = new TemplateEf
        {
            Id = domainTemplate.Id,
            UserId = domainTemplate.UserId,
            Message = domainTemplate.Message,
            CreatedAt = domainTemplate.CreatedAt,
            UpdatedAt = domainTemplate.UpdatedAt
        };

        context.Register(domainTemplate.Id, ef);
        return ef;
    }
}