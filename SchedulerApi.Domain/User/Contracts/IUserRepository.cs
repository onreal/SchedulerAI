namespace SchedulerApi.Domain.User.Contracts
{
    public interface IUserRepository
    {
        Task<Entities.User?> GetByIdAsync(Guid id);
        Task<Entities.User?> GetByEmailAsync(string email);
        Task<Entities.User?> GetByApiKeyAsync(string apiKey);
        Task AddAsync(Entities.User user);
        Task UpdateAsync(Entities.User user);
        Task DeleteAsync(Entities.User user);
    }
}