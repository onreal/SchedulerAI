using SchedulerApi.Application.Recipient.Contracts;
using SchedulerApi.Application.Recipient.DTOs;
using SchedulerApi.Domain.Recipient.Contracts;
using SchedulerApi.Domain.Shared.Contracts;

namespace SchedulerApi.Application.Recipient.Services;

public class UpdateRecipientService(IRecipientRepository repository, IUnitOfWork unitOfWork) : IUpdateRecipientService
{
    public async Task UpdateAsync(Guid id, UpdateRecipientRequest request)
    {
        var recipient = await repository.GetByIdAsync(id);
        if (recipient is null)
            throw new KeyNotFoundException("Recipient not found.");

        recipient.Update(request.Name, request.Surname, request.Email, request.Phone);
        await repository.UpdateAsync(recipient);
        await unitOfWork.SaveChangesAsync();
    }
}