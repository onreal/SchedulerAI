using SchedulerApi.Application.UserIntegration.Contracts;
using SchedulerApi.Application.UserIntegration.DTOs;
using SchedulerApi.Domain.Shared.Contracts;
using SchedulerApi.Domain.UserIntegration.Contracts;

namespace SchedulerApi.Application.UserIntegration.Services;

public class UpdateUserIntegrationService(IUserIntegrationRepository repository, IUnitOfWork unitOfWork)
    : IUpdateUserIntegrationService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;


    public async Task UpdateAsync(Guid id, UpdateUserIntegrationRequest request)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity == null) throw new KeyNotFoundException("UserIntegration not found.");

        entity.Update(request.Type, request.Settings);
        await repository.UpdateAsync(entity);
    }
}