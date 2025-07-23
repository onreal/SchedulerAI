using SchedulerApi.Domain.Shared.Contracts;

namespace SchedulerApi.Infrastructure.Persistence.Entities;

public class TemplateEf : IUserScopedEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public UserEf User { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}