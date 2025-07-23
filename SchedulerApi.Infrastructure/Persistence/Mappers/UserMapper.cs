using SchedulerApi.Domain.User.Entities;
using SchedulerApi.Domain.ValueObjects;
using SchedulerApi.Infrastructure.Persistence.Accessors;
using SchedulerApi.Infrastructure.Persistence.Entities;

namespace SchedulerApi.Infrastructure.Persistence.Mappers;

public static class UserMapper
{
    public static User ToDomain(this UserEf efUser)
    {
        if (efUser == null) return null;
        
        return User.Rehydrate(
            efUser.Id,
            efUser.Name,
            efUser.Surname,
            efUser.Email,
            Plan.FromKey(efUser.Plan),
            efUser.ApiKey,
            efUser.CreatedAt,
            efUser.UsageCount);
    }

    public static UserEf ToEfEntity(this User domainUser, MappingContextAccessor context)
    {
        if (domainUser == null) return null;

        var cached = context.Get<UserEf>(domainUser.Id);
        if (cached != null)
        {
            // Update fields if needed
            cached.Name = domainUser.Name;
            cached.Surname = domainUser.Surname;
            cached.Email = domainUser.Email;
            cached.Plan = domainUser.Plan.Key;
            cached.ApiKey = domainUser.ApiKey;
            cached.UsageCount = domainUser.UsageCount;
            return cached;
        }

        var ef = new UserEf
        {
            Id = domainUser.Id,
            Name = domainUser.Name,
            Surname = domainUser.Surname,
            Email = domainUser.Email,
            Plan = domainUser.Plan.Key,
            ApiKey = domainUser.ApiKey,
            CreatedAt = domainUser.CreatedAt,
            UsageCount = domainUser.UsageCount
        };

        context.Register(domainUser.Id, ef);
        return ef;
    }
}