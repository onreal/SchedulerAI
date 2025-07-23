using SchedulerApi.Application.Recipient.Contracts;
using SchedulerApi.Application.Recipient.DTOs;
using SchedulerApi.Domain.Recipient.Contracts;

namespace SchedulerApi.Application.Recipient.Services;

public class GetRecipientService(IRecipientRepository repository) : IGetRecipientService
{
    public async Task<IEnumerable<GetRecipient>> GetAllAsync()
    {
        var recipients = await repository.GetAllAsync();
        return recipients.Select(GetRecipient.FromDomain).Where(dto => dto != null)!;
    }

    public async Task<GetRecipient?> GetByIdAsync(Guid id)
    {
        var efRecipient = await repository.GetByIdAsync(id);

        if (efRecipient == null)
            return null;

        return GetRecipient.FromDomain(efRecipient)!;
    }
}