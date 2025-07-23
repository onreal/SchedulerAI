using SchedulerApi.Application.Recipient.Contracts;
using SchedulerApi.Application.Recipient.DTOs;
using SchedulerApi.Domain.Recipient.Contracts;
using SchedulerApi.Domain.Shared.Contracts;

namespace SchedulerApi.Application.Recipient.Services;

public class CreateRecipientService(IRecipientRepository repository, IUnitOfWork unitOfWork) : ICreateRecipientService
{
    public async Task<Domain.Recipient.Recipient> CreateAsync(CreateRecipientRequest request)
    {
        var recipient = Domain.Recipient.Recipient.Create(
            name: request.Name,
            surname: request.Surname,
            phone: request.Phone,
            email: request.Email
        );
        
        recipient = await repository.AddAsync(recipient);
        await unitOfWork.SaveChangesAsync();

        return recipient;
    }
}