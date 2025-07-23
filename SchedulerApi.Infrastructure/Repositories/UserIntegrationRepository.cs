using Microsoft.EntityFrameworkCore;
using SchedulerApi.Domain.UserIntegration;
using SchedulerApi.Domain.UserIntegration.Contracts;
using SchedulerApi.Infrastructure.Persistence;
using SchedulerApi.Infrastructure.Persistence.Accessors;
using SchedulerApi.Infrastructure.Persistence.Mappers;

namespace SchedulerApi.Infrastructure.Repositories;

public class UserIntegrationRepository : IUserIntegrationRepository
{
    private readonly SchedulerDbContext _context;
    private readonly MappingContextAccessor _mapping;

    public UserIntegrationRepository(SchedulerDbContext context, MappingContextAccessor mapping)
    {
        _context = context;
        _mapping = mapping;
    }

    public async Task<UserIntegration?> GetByIdAsync(Guid id)
    {
        var efEntity = await _context.UserIntegrations.FindAsync(id);
        return efEntity?.ToDomain(); // Implement ToDomain() mapping extension
    }

    public async Task<IEnumerable<UserIntegration>> GetByUserIdAsync(Guid userId)
    {
        var efList = await _context.UserIntegrations
            .Where(ui => ui.UserId == userId)
            .ToListAsync();

        return efList.Select(ui => ui.ToDomain()).ToList();
    }

    public async Task<IEnumerable<UserIntegration>> GetByIntegrationAsync(string integrationId)
    {
        var efList = await _context.UserIntegrations
            .Where(ui => ui.IntegrationId == integrationId)
            .ToListAsync();

        return efList.Select(ui => ui.ToDomain()).ToList();
    }

    public async Task<UserIntegration> AddAsync(UserIntegration integration)
    {
        await _context.UserIntegrations.AddAsync(integration.ToEfEntity(_mapping));
        var ef = integration.ToEfEntity(_mapping);
        _mapping.Register(ef.Id, ef);
        await _context.UserIntegrations.AddAsync(ef);

        return ef.ToDomain();
    }

    public Task UpdateAsync(UserIntegration integration)
    {
        _context.UserIntegrations.Update(integration.ToEfEntity(_mapping));
        return Task.CompletedTask;
    }

    public Task DeleteAsync(UserIntegration integration)
    {
        _context.UserIntegrations.Remove(integration.ToEfEntity(_mapping));
        return Task.CompletedTask;
    }
    
    public async Task<IEnumerable<UserIntegration>> GetAllAsync()
    {
        var efList = await _context.UserIntegrations.ToListAsync();

        foreach (var ef in efList)
            _mapping.Register(ef.Id, ef);

        return efList.Select(r => r.ToDomain());
    }
}