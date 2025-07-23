using SchedulerApi.Domain.Recipient;
using SchedulerApi.Infrastructure.Persistence.Accessors;
using SchedulerApi.Infrastructure.Persistence.Entities;

namespace SchedulerApi.Infrastructure.Persistence.Mappers;

public static class RecipientMapper
{
    public static Recipient ToDomain(this RecipientEf efRecipient)
    {
        if (efRecipient == null) return null;
        
        return Recipient.Rehydrate(
            efRecipient.Id,
            efRecipient.UserId,
            efRecipient.Name,
            efRecipient.Surname,
            efRecipient.Phone,
            efRecipient.Email,
            efRecipient.CreatedAt);
    }

    public static RecipientEf ToEfEntity(this Recipient domainRecipient, MappingContextAccessor context)
    {
        if (domainRecipient == null) return null;

        var cached = context.Get<RecipientEf>(domainRecipient.Id);
        if (cached != null)
        {
            cached.UserId = domainRecipient.UserId;
            cached.Name = domainRecipient.Name;
            cached.Surname = domainRecipient.Surname;
            cached.Phone = domainRecipient.Phone;
            cached.Email = domainRecipient.Email;
            cached.CreatedAt = domainRecipient.CreatedAt;
            return cached;
        }

        var ef = new RecipientEf
        {
            Id = domainRecipient.Id,
            UserId = domainRecipient.UserId,
            Name = domainRecipient.Name,
            Surname = domainRecipient.Surname,
            Phone = domainRecipient.Phone,
            Email = domainRecipient.Email,
            CreatedAt = domainRecipient.CreatedAt
        };

        context.Register(domainRecipient.Id, ef);
        return ef;
    }
}