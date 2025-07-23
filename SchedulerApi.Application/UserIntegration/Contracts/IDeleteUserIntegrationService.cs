namespace SchedulerApi.Application.UserIntegration.Contracts;

public interface IDeleteUserIntegrationService
{
    Task DeleteAsync(Guid id);
}