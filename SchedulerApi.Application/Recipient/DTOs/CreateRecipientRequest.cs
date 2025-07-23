namespace SchedulerApi.Application.Recipient.DTOs;

public record CreateRecipientRequest(string Name, string Surname, string Phone, string Email);