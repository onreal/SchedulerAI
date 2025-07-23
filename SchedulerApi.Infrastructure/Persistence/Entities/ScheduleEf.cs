using SchedulerApi.Domain.Shared.Contracts;

namespace SchedulerApi.Infrastructure.Persistence.Entities;

public class ScheduleEf : IUserScopedEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public UserEf User { get; set; }
    public List<string>? IntegrationIds { get; set; }
    public List<Guid> RecipientIds { get; set; }
    public DateTime EventTime { get; set; }
    public int NotifyBeforeMinutes { get; set; }
    public bool IsSent { get; set; }
    public DateTime? SentAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}