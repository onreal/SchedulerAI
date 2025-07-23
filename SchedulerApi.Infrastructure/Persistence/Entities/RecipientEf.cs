using SchedulerApi.Domain.Shared.Contracts;

namespace SchedulerApi.Infrastructure.Persistence.Entities;

public class RecipientEf : IUserScopedEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public UserEf User { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
}