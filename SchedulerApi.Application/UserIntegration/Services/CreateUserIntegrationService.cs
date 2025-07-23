using SchedulerApi.Application.UserIntegration.Contracts;
using SchedulerApi.Application.UserIntegration.DTOs;
using SchedulerApi.Domain.Shared.Contracts;
using SchedulerApi.Domain.UserIntegration.Contracts;

namespace SchedulerApi.Application.UserIntegration.Services;

public class CreateUserIntegrationService(IUserIntegrationRepository repository, IUnitOfWork unitOfWork)
    : ICreateUserIntegrationService
{
    public async Task<Domain.UserIntegration.UserIntegration> CreateAsync(CreateUserIntegrationRequest request)
    {
        var entity = Domain.UserIntegration.UserIntegration.Create(request.TemplateId, request.Order, request.IntegrationId, request.Settings);
        var template = await repository.AddAsync(entity);
        await unitOfWork.SaveChangesAsync();

        return template;
    }
}