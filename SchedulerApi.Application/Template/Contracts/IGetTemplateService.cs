using SchedulerApi.Application.Template.DTOs;

namespace SchedulerApi.Application.Template.Contracts;

public interface IGetTemplateService
{
    Task<GetTemplate?> GetByIdAsync(Guid id);
    Task<IEnumerable<GetTemplate>> GetAllAsync();
}