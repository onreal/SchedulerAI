using SchedulerApi.Domain.Shared.Contracts;

namespace SchedulerApi.Infrastructure.Persistence.Entities;

public class UserIntegrationEf : IUserScopedEntity
{
    public Guid Id { get; init; }
    public Guid TemplateId { get; set; }
    public TemplateEf Template { get; set; }
    public int Order { get; set; }
    public Guid UserId { get; set; }
    public UserEf User { get; set; }
    public string IntegrationId { get; set; }
    public string Settings { get; set; }
}