using SchedulerApi.Application.Recipient.DTOs;

namespace SchedulerApi.Application.Recipient.Contracts;

public interface ICreateRecipientService
{
    Task<Domain.Recipient.Recipient> CreateAsync(CreateRecipientRequest request);
}