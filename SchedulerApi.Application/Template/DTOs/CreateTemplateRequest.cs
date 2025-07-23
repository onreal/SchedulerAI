namespace SchedulerApi.Application.Template.DTOs;

public record CreateTemplateRequest(string IntegrationId, string Name, string Message);