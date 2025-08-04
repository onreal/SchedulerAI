namespace SchedulerApi.Domain.Recipient.Contracts;

public interface IRecipientRepository
{
    Task<Recipient?> GetByIdAsync(Guid id);
    Task<Recipient?> GetByNameAsync(string name);
    Task<IEnumerable<Recipient>> GetByIdsAsync(List<Guid> id);
    Task<IEnumerable<Recipient>> GetByUserIdAsync(Guid userId);
    Task<Recipient> AddAsync(Recipient recipient);
    Task UpdateAsync(Recipient recipient);
    Task DeleteAsync(Recipient recipient);
    Task<IEnumerable<Recipient>> GetAllAsync();
}