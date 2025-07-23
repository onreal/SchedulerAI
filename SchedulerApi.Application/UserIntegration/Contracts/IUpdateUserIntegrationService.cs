using SchedulerApi.Application.UserIntegration.DTOs;

namespace SchedulerApi.Application.UserIntegration.Contracts;

public interface IUpdateUserIntegrationService
{
    Task UpdateAsync(Guid id, UpdateUserIntegrationRequest request);
}