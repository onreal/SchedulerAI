using SchedulerApi.Domain.Shared.Contracts;

namespace SchedulerApi.Infrastructure.Persistence;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly SchedulerDbContext _context;

    public EfUnitOfWork(SchedulerDbContext context)
    {
        _context = context;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);
}