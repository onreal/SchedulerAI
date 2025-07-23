namespace SchedulerApi.Application.Recipient.DTOs;

public record UpdateRecipientRequest(string Name, string Surname, string Phone, string Email);