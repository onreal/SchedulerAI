using SchedulerApi.Application.Template.DTOs;

namespace SchedulerApi.Application.Template.Contracts;

public interface IUpdateTemplateService
{
    Task UpdateAsync(Guid id, UpdateTemplateRequest request);
}