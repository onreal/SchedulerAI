namespace SchedulerApi.Application.UserIntegration.DTOs;

public record GetUserIntegration(Guid Id, Guid UserId, string Type, string Settings)
{
    public static GetUserIntegration? FromDomain(Domain.UserIntegration.UserIntegration? domain)
    {
        if (domain == null) return null;
        return new GetUserIntegration(domain.Id, domain.UserId, domain.IntegrationId, domain.Settings);
    }
}