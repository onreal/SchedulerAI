using SchedulerApi.Application.UserIntegration.DTOs;

namespace SchedulerApi.Application.UserIntegration.Contracts;

public interface ICreateUserIntegrationService
{
    Task<Domain.UserIntegration.UserIntegration> CreateAsync(CreateUserIntegrationRequest request);
}