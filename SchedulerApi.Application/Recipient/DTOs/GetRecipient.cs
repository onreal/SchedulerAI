namespace SchedulerApi.Application.Recipient.DTOs;

public record GetRecipient(
    Guid Id,
    string Name,
    string Surname,
    string Phone,
    string Email,
    DateTime CreatedAt)
{
    public static GetRecipient FromDomain(Domain.Recipient.Recipient domain)
    {
        return new GetRecipient(
            domain.Id,
            domain.Name,
            domain.Surname,
            domain.Phone,
            domain.Email,
            domain.CreatedAt);
    }
}