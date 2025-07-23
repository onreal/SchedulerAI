using SchedulerApi.Application.Template.Contracts;
using SchedulerApi.Application.Template.DTOs;
using SchedulerApi.Domain.Shared.Contracts;
using SchedulerApi.Domain.Template.Contracts;

namespace SchedulerApi.Application.Template.Services;

public class CreateTemplateService(ITemplateRepository repository, IUnitOfWork unitOfWork) : ICreateTemplateService
{
    public async Task CreateAsync(CreateTemplateRequest request)
    {
        var template = Domain.Template.Template.Create(
            request.Message
        );

        await repository.AddAsync(template);
        await unitOfWork.SaveChangesAsync();
    }
}