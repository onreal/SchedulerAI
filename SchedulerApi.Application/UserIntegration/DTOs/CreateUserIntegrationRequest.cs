namespace SchedulerApi.Application.UserIntegration.DTOs;

public record CreateUserIntegrationRequest(Guid TemplateId, int Order, string IntegrationId, string Settings);