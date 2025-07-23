using Microsoft.EntityFrameworkCore;
using SchedulerApi.Domain.User.Contracts;
using SchedulerApi.Domain.User.Entities;
using SchedulerApi.Infrastructure.Persistence;
using SchedulerApi.Infrastructure.Persistence.Accessors;
using SchedulerApi.Infrastructure.Persistence.Mappers;

namespace SchedulerApi.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SchedulerDbContext _context;
    private readonly MappingContextAccessor _mappingContextAccessor;

    public UserRepository(SchedulerDbContext context, MappingContextAccessor mappingContextAccessor)
    {
        _context = context;
        _mappingContextAccessor = mappingContextAccessor;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        var userEf = await _context.Users.FindAsync(id);
        if (userEf == null) return null;

        _mappingContextAccessor.Register(userEf.Id, userEf);
        return userEf.ToDomain();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var userEf = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (userEf == null) return null;

        _mappingContextAccessor.Register(userEf.Id, userEf);
        return userEf.ToDomain();
    }

    public async Task<User?> GetByApiKeyAsync(string apiKey)
    {
        var userEf = await _context.Users.FirstOrDefaultAsync(u => u.ApiKey == apiKey);
        if (userEf == null) return null;

        _mappingContextAccessor.Register(userEf.Id, userEf);
        return userEf.ToDomain();
    }

    public async Task AddAsync(User user)
    {
        var userEf = user.ToEfEntity(_mappingContextAccessor);
        _mappingContextAccessor.Register(userEf.Id, userEf);
        await _context.Users.AddAsync(userEf);
    }

    public Task UpdateAsync(User user)
    {
        var userEf = user.ToEfEntity(_mappingContextAccessor);
        _mappingContextAccessor.Register(userEf.Id, userEf);
        _context.Users.Update(userEf);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(User user)
    {
        var userEf = user.ToEfEntity(_mappingContextAccessor);
        _mappingContextAccessor.Register(userEf.Id, userEf);
        _context.Users.Remove(userEf);
        return Task.CompletedTask;
    }
}
