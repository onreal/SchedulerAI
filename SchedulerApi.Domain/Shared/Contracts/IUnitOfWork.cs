namespace SchedulerApi.Domain.Shared.Contracts;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}