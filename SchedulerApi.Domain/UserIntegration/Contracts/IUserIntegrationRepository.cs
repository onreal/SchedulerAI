namespace SchedulerApi.Domain.UserIntegration.Contracts
{
    public interface IUserIntegrationRepository
    {
        Task<UserIntegration?> GetByIdAsync(Guid id);
        Task<IEnumerable<UserIntegration>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<UserIntegration>> GetByIntegrationAsync(string integrationId);
        Task<UserIntegration> AddAsync(UserIntegration integration);
        Task UpdateAsync(UserIntegration integration);
        Task DeleteAsync(UserIntegration integration);
        Task<IEnumerable<UserIntegration>> GetAllAsync();
    }
}