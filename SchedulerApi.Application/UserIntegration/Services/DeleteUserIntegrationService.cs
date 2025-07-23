using SchedulerApi.Application.UserIntegration.Contracts;
using SchedulerApi.Domain.Shared.Contracts;
using SchedulerApi.Domain.UserIntegration.Contracts;

namespace SchedulerApi.Application.UserIntegration.Services;

public class DeleteUserIntegrationService(IUserIntegrationRepository repository, IUnitOfWork unitOfWork)
    : IDeleteUserIntegrationService
{
    public async Task DeleteAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity == null) throw new KeyNotFoundException("UserIntegration not found.");

        await repository.DeleteAsync(entity);
        await unitOfWork.SaveChangesAsync();
    }
}