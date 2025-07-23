using SchedulerApi.Application.Template.DTOs;

namespace SchedulerApi.Application.Template.Contracts;

public interface ICreateTemplateService
{
    Task CreateAsync(CreateTemplateRequest request);
}