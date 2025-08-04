using Microsoft.EntityFrameworkCore;
using SchedulerApi.Domain.Recipient;
using SchedulerApi.Domain.Recipient.Contracts;
using SchedulerApi.Infrastructure.Persistence;
using SchedulerApi.Infrastructure.Persistence.Accessors;
using SchedulerApi.Infrastructure.Persistence.Mappers;

namespace SchedulerApi.Infrastructure.Repositories;

public class RecipientRepository : IRecipientRepository
{
    private readonly SchedulerDbContext _context;
    private readonly MappingContextAccessor _mapping;

    public RecipientRepository(SchedulerDbContext context, MappingContextAccessor mapping)
    {
        _context = context;
        _mapping = mapping;
    }

    public async Task<Recipient?> GetByIdAsync(Guid id)
    {
        var ef = await _context.Recipients.FindAsync(id);
        if (ef != null) _mapping.Register(ef.Id, ef);
        return ef?.ToDomain();
    }
    
    public async Task<Recipient?> GetByNameAsync(string name)
    {
        var ef = await _context.Recipients.SingleOrDefaultAsync(r => r.Name == name);
        if (ef != null) _mapping.Register(ef.Id, ef);
        return ef?.ToDomain();
    }

    public async Task<IEnumerable<Recipient>> GetByIdsAsync(List<Guid> ids)
    {
        var efList = await _context.Recipients
            .Where(r => ids.Contains(r.Id))
            .ToListAsync();

        foreach (var ef in efList)
            _mapping.Register(ef.Id, ef);

        return efList.Select(r => r.ToDomain()).ToList();
    }

    public async Task<IEnumerable<Recipient>> GetByUserIdAsync(Guid userId)
    {
        var efList = await _context.Recipients
            .Where(r => r.UserId == userId)
            .ToListAsync();

        foreach (var ef in efList)
            _mapping.Register(ef.Id, ef);

        return efList.Select(r => r.ToDomain()).ToList();
    }

    public async Task<Recipient> AddAsync(Recipient recipient)
    {
        var ef = recipient.ToEfEntity(_mapping);
        await _context.Recipients.AddAsync(recipient.ToEfEntity(_mapping));

        return ef.ToDomain();
    }

    public Task UpdateAsync(Recipient recipient)
    {
        _context.Recipients.Update(recipient.ToEfEntity(_mapping));
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Recipient recipient)
    {
        _context.Recipients.Remove(recipient.ToEfEntity(_mapping));
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<Recipient>> GetAllAsync()
    {
        var efList = await _context.Recipients.ToListAsync();

        foreach (var ef in efList)
            _mapping.Register(ef.Id, ef);

        return efList.Select(r => r.ToDomain());
    }
}