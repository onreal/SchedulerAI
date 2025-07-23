namespace SchedulerApi.Application.Template.DTOs;

public record GetTemplate(
    Guid Id,
    Guid UserId,
    string Message,
    DateTime CreatedAt,
    DateTime UpdatedAt
)
{
    public static GetTemplate FromDomain(Domain.Template.Template template) => new(
        template.Id,
        template.UserId,
        template.Message,
        template.CreatedAt,
        template.UpdatedAt
    );
}