using SchedulerApi.Application.UserIntegration.DTOs;

namespace SchedulerApi.Application.UserIntegration.Contracts;

public interface IGetUserIntegrationService
{
    Task<IEnumerable<GetUserIntegration>> GetAllAsync();
    Task<GetUserIntegration?> GetByIdAsync(Guid id);
}