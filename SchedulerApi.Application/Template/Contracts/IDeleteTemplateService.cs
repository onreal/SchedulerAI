namespace SchedulerApi.Application.Template.Contracts;

public interface IDeleteTemplateService
{
    Task DeleteAsync(Guid id);
}