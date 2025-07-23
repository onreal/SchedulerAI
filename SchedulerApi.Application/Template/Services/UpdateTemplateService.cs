using SchedulerApi.Application.Template.Contracts;
using SchedulerApi.Application.Template.DTOs;
using SchedulerApi.Domain.Shared.Contracts;
using SchedulerApi.Domain.Template.Contracts;

namespace SchedulerApi.Application.Template.Services;

public class UpdateTemplateService(ITemplateRepository repository, IUnitOfWork unitOfWork) : IUpdateTemplateService
{
    public async Task UpdateAsync(Guid id, UpdateTemplateRequest request)
    {
        var template = await repository.GetByIdAsync(id);
        if (template is null)
            throw new KeyNotFoundException("Template not found.");

        template.Update(request.Message);
        await repository.UpdateAsync(template);
        await unitOfWork.SaveChangesAsync();
    }
}