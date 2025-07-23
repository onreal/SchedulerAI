using SchedulerApi.Application.Recipient.DTOs;

namespace SchedulerApi.Application.Recipient.Contracts;

public interface IUpdateRecipientService
{
    Task UpdateAsync(Guid id, UpdateRecipientRequest request);
}