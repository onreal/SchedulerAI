using SchedulerApi.Application.Recipient.Contracts;
using SchedulerApi.Domain.Recipient.Contracts;
using SchedulerApi.Domain.Shared.Contracts;

namespace SchedulerApi.Application.Recipient.Services;

public class DeleteRecipientService(IRecipientRepository repository, IUnitOfWork unitOfWork) : IDeleteRecipientService
{
    public async Task DeleteAsync(Guid id)
    {
        var recipient = await repository.GetByIdAsync(id);
        if (recipient is null)
            throw new KeyNotFoundException("Recipient not found.");

        await repository.DeleteAsync(recipient);
        await unitOfWork.SaveChangesAsync();
    }
}