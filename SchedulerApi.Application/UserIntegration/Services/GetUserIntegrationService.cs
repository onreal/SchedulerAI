using SchedulerApi.Application.UserIntegration.Contracts;
using SchedulerApi.Application.UserIntegration.DTOs;
using SchedulerApi.Domain.UserIntegration.Contracts;

namespace SchedulerApi.Application.UserIntegration.Services;

public class GetUserIntegrationService(IUserIntegrationRepository repository) : IGetUserIntegrationService
{
    public async Task<IEnumerable<GetUserIntegration>> GetAllAsync()
    {
        var integrations = await repository.GetAllAsync();
        return integrations
            .Select(GetUserIntegration.FromDomain)
            .Where(dto => dto != null)!;
    }

    public async Task<GetUserIntegration?> GetByIdAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        return GetUserIntegration.FromDomain(entity);
    }
}