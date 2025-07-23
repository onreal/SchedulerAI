using SchedulerApi.Application.Template.Contracts;
using SchedulerApi.Domain.Shared.Contracts;
using SchedulerApi.Domain.Template.Contracts;

namespace SchedulerApi.Application.Template.Services;

public class DeleteTemplateService(ITemplateRepository repository, IUnitOfWork unitOfWork) : IDeleteTemplateService
{
    public async Task DeleteAsync(Guid id)
    {
        var template = await repository.GetByIdAsync(id);
        if (template is null)
            throw new KeyNotFoundException("Recipient not found.");

        await repository.DeleteAsync(template);
        await unitOfWork.SaveChangesAsync();
    }
}