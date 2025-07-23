namespace SchedulerApi.Domain.Shared.Contracts;

public interface IUserScopedEntity
{
    Guid UserId { get; set; }
}