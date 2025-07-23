using SchedulerApi.Application.Recipient.DTOs;

namespace SchedulerApi.Application.Recipient.Contracts;

public interface IGetRecipientService
{
    Task<GetRecipient?> GetByIdAsync(Guid id);
    Task<IEnumerable<GetRecipient>> GetAllAsync();
}