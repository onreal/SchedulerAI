using SchedulerApi.Application.Template.Contracts;
using SchedulerApi.Application.Template.DTOs;
using SchedulerApi.Domain.Template.Contracts;

namespace SchedulerApi.Application.Template.Services;

public class GetTemplateService(ITemplateRepository repository) : IGetTemplateService
{
    public async Task<IEnumerable<GetTemplate>> GetAllAsync()
    {
        var templates = await repository.GetAllAsync();
        return templates.Select(GetTemplate.FromDomain).Where(dto => dto != null)!;
    }

    public async Task<GetTemplate?> GetByIdAsync(Guid id)
    {
        var template = await repository.GetByIdAsync(id);
        return template == null ? null : GetTemplate.FromDomain(template)!;
    }
}