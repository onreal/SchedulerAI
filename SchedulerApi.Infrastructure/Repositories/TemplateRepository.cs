using Microsoft.EntityFrameworkCore;
using SchedulerApi.Domain.Template;
using SchedulerApi.Domain.Template.Contracts;
using SchedulerApi.Infrastructure.Persistence;
using SchedulerApi.Infrastructure.Persistence.Accessors;
using SchedulerApi.Infrastructure.Persistence.Mappers;

namespace SchedulerApi.Infrastructure.Repositories;

public class TemplateRepository : ITemplateRepository
{
    private readonly SchedulerDbContext _context;
    private readonly MappingContextAccessor _mapping;

    public TemplateRepository(SchedulerDbContext context, MappingContextAccessor mapping)
    {
        _context = context;
        _mapping = mapping;
    }

    public async Task<Template?> GetByIdAsync(Guid id)
    {
        var ef = await _context.Templates.FindAsync(id);
        if (ef != null) _mapping.Register(ef.Id, ef);
        return ef?.ToDomain();
    }

    public async Task<IEnumerable<Template>> GetAllAsync()
    {
        var efList = await _context.Templates.ToListAsync();

        foreach (var ef in efList)
            _mapping.Register(ef.Id, ef);

        return efList.Select(t => t.ToDomain());
    }

    public async Task<IEnumerable<Template>> GetByUserIdAsync(Guid userId)
    {
        var efList = await _context.Templates
            .Where(t => t.UserId == userId)
            .ToListAsync();

        foreach (var ef in efList)
            _mapping.Register(ef.Id, ef);

        return efList.Select(t => t.ToDomain());
    }

    public async Task AddAsync(Template template)
    {
        var ef = template.ToEfEntity(_mapping);
        await _context.Templates.AddAsync(ef);
    }

    public Task UpdateAsync(Template template)
    {
        var ef = template.ToEfEntity(_mapping);
        _context.Templates.Update(ef);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Template template)
    {
        var ef = template.ToEfEntity(_mapping);
        _context.Templates.Remove(ef);
        return Task.CompletedTask;
    }
}